using Gukv.UiTests.Configuration;
using Gukv.UiTests.Pages;
using Microsoft.Playwright.NUnit;
using NUnit.Framework.Interfaces;

namespace Gukv.UiTests.Infrastructure;

public abstract class GukvPageTest : PageTest
{
    protected TestSettings Settings { get; } = new();

    [SetUp]
    public void ConfigurePage()
    {
        Page.SetDefaultTimeout(Settings.TimeoutMilliseconds);
        Page.SetDefaultNavigationTimeout(Settings.TimeoutMilliseconds);
    }

    [TearDown]
    public async Task CaptureFailureAsync()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
        {
            return;
        }

        string directory = Path.Combine(TestContext.CurrentContext.WorkDirectory, "test-results");
        Directory.CreateDirectory(directory);

        string testName = string.Concat(TestContext.CurrentContext.Test.Name.Select(character =>
            Path.GetInvalidFileNameChars().Contains(character) ? '_' : character));

        await Page.ScreenshotAsync(new()
        {
            Path = Path.Combine(directory, testName + ".png"),
            FullPage = true
        });
    }

    protected async Task LoginAsync()
    {
        (string username, string password) = Settings.RequireCredentials();
        await new LoginPage(Page, Settings.BaseUri).LoginAsync(username, password);
    }
}
