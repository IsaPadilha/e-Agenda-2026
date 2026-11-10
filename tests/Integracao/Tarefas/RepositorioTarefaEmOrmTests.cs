using eAgenda.Dominio.Modulos.Tarefas;
using eAgenda.Infraestrutura.Modulos.Tarefas;


namespace eAgenda.Testes.Integracao.Tarefas;

[TestClass]
public sealed partial class RepositorioTarefaEmOrmTests : RepositorioEmOrmTestsBase
{

    [TestMethod]
    public void Cadastrar_DevePersistir_TarefaEItensNoBanco()
    {
        // Arranjo
        Guid tarefaId = Guid.CreateVersion7();

        Tarefa tarefa = new Tarefa("Preparar Apresentação", PrioridadeTarefa.Alta)
        {
            Id = tarefaId,
            UsuarioId = usuarioId,
            DataCriacao = new DateOnly(2026, 11, 03)
        };

        tarefa.AdicionarItem(new ItemTarefa(tarefa, "Revisar conteúdo") { UsuarioId = usuarioId });
        tarefa.AdicionarItem(new ItemTarefa(tarefa, "Montar slides") { UsuarioId = usuarioId, Concluido = true });

        RepositorioTarefaEmOrm repositorioTarefa = new RepositorioTarefaEmOrm(dbContext);

        // Ação
        repositorioTarefa.Cadastrar(tarefa);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefaId);

        // Asserção
    }
}

