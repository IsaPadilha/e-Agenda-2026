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
}