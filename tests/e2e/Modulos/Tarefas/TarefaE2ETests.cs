using eAgenda.Testes.e2e.Compartilhado;
using Microsoft.Playwright;

namespace eAgenda.Testes.e2e.Modulos.Tarefas;

[TestClass]
public sealed class TarefaE2ETests
{
    private TestApplicationFactory applicationFactory = null!;
    private IPlaywright playwright = null!;
    private IBrowser browser = null!;
    private IBrowserContext browserContext = null!;
    private IPage page = null!;
    private string? baseUrl = null;

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

    [TestMethod]
    public async Task Deve_Cadastrar_Tarefa_Corretamente()
    {
        // Arranjo
        await page.GotoAsync($"{baseUrl}/Autenticacao/Cadastro");

        await page.GetByLabel("E-mail").FillAsync("isap4619@gmail.com");
        await page.GetByLabel("Senha", new() { Exact = true }).FillAsync("Teste@123");
        await page.GetByLabel("Confirmar Senha", new() { Exact = true }).FillAsync("Teste@123");
        await page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" }).ClickAsync();

        await page.GetByLabel("E-mail").FillAsync("isap4619@gmail.com");
        await page.GetByLabel("Senha").FillAsync("Teste@123");
        await page.GetByRole(AriaRole.Button, new() { Name = "Entrar" }).ClickAsync();

        await page.GotoAsync($"{baseUrl}/Tarefa/Cadastrar");

        // Ação
        await page.GetByLabel("Título").FillAsync("Lavar o cachorro");
        await page.GetByLabel("Prioridade").SelectOptionAsync("Alta");

        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        ILocator card = page.Locator(".card").Filter(new() { HasText = "Lavar o cachorro" });

        // Asserção
        await Assertions.Expect(card).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task Deve_Editar_Tarefa_Corretamente()
    {
        // Arranjo

        // Ação

        // Asserção
    }

    [TestMethod]
    public async Task Deve_Excluir_Tarefa_Corretamente()
    {
        // Arranjo

        // Ação

        // Asserção
    }
}