using eAgenda.Testes.e2e.Compartilhado;
using Microsoft.Playwright;

namespace eAgenda.Testes.e2e.Modulos.Tarefas;

[TestClass]
public sealed class TarefaE2ETests : E2ETestsBase
{

    [TestMethod]
    public async Task Deve_Cadastrar_Tarefa_Corretamente()
    {
        // Arranjo
        await CadastrarUsuarioAsync();

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
        await CadastrarUsuarioAsync();

        await page.GotoAsync($"{baseUrl}/Tarefa/Cadastrar");

        await page.GetByLabel("Título").FillAsync("Lavar o cachorro");
        await page.GetByLabel("Prioridade").SelectOptionAsync("Alta");

        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        ILocator card = page.Locator(".card").Filter(new() { HasText = "Lavar o cachorro" });

        await card.GetByRole(AriaRole.Link, new() { Name = "Editar", Exact = true }).ClickAsync();

        // Ação
        await page.GetByLabel("Título").FillAsync("Passear com o cachorro");
        await page.GetByLabel("Prioridade").SelectOptionAsync("Baixa");

        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        ILocator cardEditado = page.Locator(".card").Filter(new() { HasText = "Passear com o cachorro" });

        // Asserção
        await Assertions.Expect(cardEditado).ToHaveCountAsync(1);
        await Assertions.Expect(cardEditado.Locator(".card-title")).ToHaveTextAsync("Passear com o cachorro");
        await Assertions.Expect(cardEditado.GetByText("Baixa", new() { Exact = true })).ToBeVisibleAsync();
        await Assertions.Expect(card).ToHaveCountAsync(0);
    }

    [TestMethod]
    public async Task Deve_Excluir_Tarefa_Corretamente()
    {
        // Arranjo
        await CadastrarUsuarioAsync();

        await page.GotoAsync($"{baseUrl}/Tarefa/Cadastrar");

        await page.GetByLabel("Título").FillAsync("Lavar o cachorro");
        await page.GetByLabel("Prioridade").SelectOptionAsync("Alta");

        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        ILocator card = page.Locator(".card").Filter(new() { HasText = "Lavar o cachorro" });

        await card.GetByRole(AriaRole.Link, new() { Name = "Excluir", Exact = true }).ClickAsync();

        // Ação
        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        // Asserção
        await Assertions.Expect(card).ToHaveCountAsync(0);
        await Assertions.Expect(page.GetByRole(AriaRole.Alert)).ToHaveTextAsync("Nenhuma tarefa cadastrada.");
    }
}