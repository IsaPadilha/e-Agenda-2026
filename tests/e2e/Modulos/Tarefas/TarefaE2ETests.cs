using Microsoft.Playwright;

namespace eAgenda.Testes.e2e.Modulos.Tarefas;

[TestClass]
public sealed class TarefaE2ETests
{
    private IPlaywright playwright = null!;
    private IBrowser browser = null!;
    private IBrowserContext browserContext = null!;
    private IPage page = null!;

    [TestInitialize]
    public async Task InicializarTeste()
    {
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
    }

    [TestMethod]
    public async Task Deve_Cadastrar_Tarefa_Corretamente()
    {
        // Arranjo
        await page.GotoAsync("http://localhost:5013/Tarefa/Cadastrar");

        await page.GetByLabel("E-mail").FillAsync("tiago@academiadoprogramador.net");
        await page.GetByLabel("Senha").FillAsync("Teste@123");
        await page.GetByRole(AriaRole.Button, new() { Name = "Entrar" }).ClickAsync();

        await page.GotoAsync("http://localhost:5013/Tarefa/Cadastrar");

        // Ação
        await page.GetByLabel("Título").FillAsync("Lavar o cachorro");
        await page.GetByLabel("Prioridade").SelectOptionAsync("Alta");

        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        ILocator card = page.Locator(".card").Filter(new() { HasText = "Lavar o cachorro" });

        // Asserção
        Assert.IsTrue(await card.First.IsVisibleAsync());
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