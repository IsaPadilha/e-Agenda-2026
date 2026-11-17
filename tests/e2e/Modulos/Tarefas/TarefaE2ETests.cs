using Microsoft.Playwright;

namespace eAgenda.Testes.e2e;

[TestClass]
public sealed class TarefaE2ETests
{
    [TestMethod]
    public async Task TestMethod1()
    {
        IPlaywright playwright = await Playwright.CreateAsync();
        IBrowser browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });

        IBrowserContext browserContext = await browser.NewContextAsync();

        IPage page = await browserContext.NewPageAsync();

        await page.GotoAsync("https://www.academiadoprogramador.net");

        await page.GetByText("Quero me inscrever").ClickAsync();

        await browser.DisposeAsync();
        playwright.Dispose();
    }
}