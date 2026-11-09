using eAgenda.Dominio.Modulos.Tarefas;

namespace eAgenda.Testes.Unidade.Tarefas;

// MSTest
// NUnit
// XUnit
[TestClass]
public sealed class TarefaTests
{
    // Casos de Teste
    // Padrão AAA (Arranjo, Ação, Asserção)
    [TestMethod]
    public void Construtor_ComDadosValidos_DeveInicializarTarefa_Pendente()
    {
        // Arranjo
        Tarefa tarefa;

        // Ação
        tarefa = new("Estudar", PrioridadeTarefa.Alta);

        // Asserção
        Assert.AreEqual("Estudar", tarefa.Titulo);
    }

    [TestMethod]
    public void AdicionarItem_Deve_RecalcularConclusao_TarefaPendente()
    {
        // Arranjo
        Tarefa tarefa = new("Estudar", PrioridadeTarefa.Alta);
        ItemTarefa item = new(tarefa, "Organizar material de estudo");
        item.Concluido = true;

        // Ação
        tarefa.AdicionarItem(item);

        // Asserção
        Assert.HasCount(1, tarefa.Itens);
        Assert.AreEqual(item, tarefa.Itens[0]);

        Assert.AreEqual(100, tarefa.PercentualConcluido);
        Assert.IsTrue(tarefa.Concluida);
        Assert.AreEqual(DateOnly.FromDateTime(DateTime.Now), tarefa.DataConclusao);
    }

    [TestMethod]
    public void RemoverItem_Deve_RecalcularConclusao_TarefaConcluida()
    {
        // Arranjo

        // Ação

        // Asserção
    }
}