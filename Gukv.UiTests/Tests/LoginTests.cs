using Gukv.UiTests.Infrastructure;
using Gukv.UiTests.Pages;
using Microsoft.Playwright;

namespace Gukv.UiTests.Tests;

[TestFixture]
[Category("ReadOnly")]
public sealed class LoginTests : GukvPageTest
{
    [Test]
    public async Task LoginPageIsRendered()
    {
        await new LoginPage(Page, Settings.BaseUri).OpenAsync();

        bool usernameIsVisible = await Page
            .GetByLabel("Ім'я користувача:", new() { Exact = true })
            .IsVisibleAsync();
        bool passwordIsVisible = await Page
            .GetByLabel("Пароль:", new() { Exact = true })
            .IsVisibleAsync();
        bool loginButtonIsVisible = await Page
            .Locator("[id$='LoginButton']")
            .First
            .IsVisibleAsync();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(usernameIsVisible, Is.True);
            Assert.That(passwordIsVisible, Is.True);
            Assert.That(loginButtonIsVisible, Is.True);
        }
    }

    [Test]
    public async Task ConfiguredUserCanLogin()
    {
        await LoginAsync();
        Assert.That(new Uri(Page.Url).AbsolutePath, Does.Not.EndWith("/Account/Login.aspx"));
    }
}
