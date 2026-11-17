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