using Microsoft.Playwright;

namespace eAgenda.Testes.e2e.Compartilhado;

public abstract class E2ETestsBase
{
    private TestApplicationFactory applicationFactory = null!;
    private IPlaywright playwright = null!;
    private IBrowser browser = null!;
    private IBrowserContext browserContext = null!;
    protected IPage page = null!;
    protected string? baseUrl = null;

    [TestInitialize]
    public async Task InicializarTeste()
    {
        applicationFactory = new TestApplicationFactory();
        await applicationFactory.StartAsync();

        baseUrl = applicationFactory.BaseUrl;

        playwright = await Playwright.CreateAsync();
        browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });

        browserContext = await browser.NewContextAsync();
        page = await browserContext.NewPageAsync();
    }

    [TestCleanup]
    public async Task FinalizarTeste()
    {
        await browser.DisposeAsync();
        playwright.Dispose();

        await applicationFactory.DisposeAsync();
    }

    protected async Task CadastrarUsuarioAsync()
    {
        await page.GotoAsync($"{baseUrl}/Autenticacao/Cadastro");

        await page.GetByLabel("E-mail").FillAsync("isap4619@gmail.com");
        await page.GetByLabel("Senha", new() { Exact = true }).FillAsync("Teste@123");
        await page.GetByLabel("Confirmar Senha", new() { Exact = true }).FillAsync("Teste@123");
        await page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" }).ClickAsync();

        await page.GetByLabel("E-mail").FillAsync("isap4619@gmail.com");
        await page.GetByLabel("Senha").FillAsync("Teste@123");
        await page.GetByRole(AriaRole.Button, new() { Name = "Entrar" }).ClickAsync();
    }
}