using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace PlaywrightTests;

[TestClass]
public class CItest{
    private IPlaywright playwright;
    private IBrowser browser;
    private IBrowserContext browserContext;
    private IPage page;

   [TestInitialize]
    public async Task Setup()
    {
        playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
            SlowMo = 1000 // Lägger in en fördröjning så vi kan se vad som händer
        });
        browserContext = await browser.NewContextAsync();
        page = await browserContext.NewPageAsync();
    }

    [TestCleanup]
    public async Task Cleanup()
    {
        await browserContext.CloseAsync();
        await browser.CloseAsync();
        playwright.Dispose();
    }

    [TestMethod]
    public async Task GetLink()
    {
        await page.GotoAsync("http://localhost:5173");
        //enter email
        await page.GetByRole(AriaRole.Textbox, new(){Name = "email"}).FillAsync("linda@exempel.se");
        //enter password
        await page.GetByRole(AriaRole.Textbox, new(){Name = "password"}).FillAsync("pass123");
        //click Login
        await page.GetByRole(AriaRole.Button, new() {Name = "Login"}).ClickAsync();
        //check user lis logged in
        await Expect(page.GetByText("Linda Larsson")).ToBeVisibleAsync();
        await Expect(page.Locator("#user-info")).toHaveText("Linda Larsson");
    }
}