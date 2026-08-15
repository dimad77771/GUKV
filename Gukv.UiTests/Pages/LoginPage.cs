using Microsoft.Playwright;

namespace Gukv.UiTests.Pages;

public sealed class LoginPage(IPage page, Uri baseUri)
{
    public async Task OpenAsync()
    {
        await page.GotoAsync(
            new Uri(baseUri, "Account/Login.aspx").AbsoluteUri,
            new() { WaitUntil = WaitUntilState.DOMContentLoaded });
    }

    public async Task LoginAsync(string username, string password)
    {
        await OpenAsync();
        await page.GetByLabel("Ім'я користувача:", new() { Exact = true }).FillAsync(username);
        await page.GetByLabel("Пароль:", new() { Exact = true }).FillAsync(password);
        await page.Locator("[id$='LoginButton']").First.ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

        if (new Uri(page.Url).AbsolutePath.EndsWith("/Account/Login.aspx", StringComparison.OrdinalIgnoreCase))
        {
            string failure = await page.Locator(".failureNotification").First.TextContentAsync() ?? string.Empty;
            throw new AssertionException("Login failed. " + failure.Trim());
        }
    }
}
