using eAgenda.Dominio.Modulos.Tarefas;
using eAgenda.Infraestrutura.Modulos.Tarefas;

namespace eAgenda.Testes.Integracao.Modulos.Tarefas;

[TestClass]
public sealed partial class RepositorioTarefaEmOrmTests : RepositorioEmOrmTestsBase
{
    [TestMethod]
    public void Cadastrar_DevePersistir_TarefaEItensNoBanco()
    {
        // Arranjo
        Guid tarefaId = Guid.CreateVersion7();

        Tarefa tarefa = new("Preparar Apresentação", PrioridadeTarefa.Alta)
        {
            Id = tarefaId,
            UsuarioId = usuarioId,
            DataCriacao = new DateOnly(2026, 11, 03)
        };

        tarefa.AdicionarItem(new ItemTarefa(tarefa, "Revisar conteúdo") { UsuarioId = usuarioId });
        tarefa.AdicionarItem(new ItemTarefa(tarefa, "Montar slides") { UsuarioId = usuarioId, Concluido = true });

        RepositorioTarefaEmOrm repositorioTarefa = new(dbContext);

        // Ação
        repositorioTarefa.Cadastrar(tarefa);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefaId);

        // Asserção
        Assert.IsNotNull(tarefaSelecionada);
        Assert.AreEqual("Preparar Apresentação", tarefaSelecionada.Titulo);
        Assert.AreEqual(PrioridadeTarefa.Alta, tarefaSelecionada.Prioridade);
        Assert.AreEqual(new DateOnly(2026, 11, 03), tarefaSelecionada.DataCriacao);
        Assert.AreEqual(50, tarefaSelecionada.PercentualConcluido);
        Assert.HasCount(2, tarefaSelecionada.Itens);

        CollectionAssert.AreEqual(
            new[] { "Montar slides", "Revisar conteúdo" },
            tarefaSelecionada.Itens.Select(i => i.Titulo).ToArray()
        );
    }

    [TestMethod]
    public void Editar_DeveAtualizar_TarefaEItensNoBanco()
    {
        // Arranjo
        Guid tarefaId = Guid.CreateVersion7();

        Tarefa tarefaOriginal = new("Preparar Apresentação", PrioridadeTarefa.Alta)
        {
            Id = tarefaId,
            UsuarioId = usuarioId,
            DataCriacao = new DateOnly(2026, 11, 03)
        };

        tarefaOriginal.AdicionarItem(new ItemTarefa(tarefaOriginal, "Revisar conteúdo") { UsuarioId = usuarioId });

        RepositorioTarefaEmOrm repositorioTarefa = new(dbContext);

        repositorioTarefa.Cadastrar(tarefaOriginal);
        dbContext.ChangeTracker.Clear();

        Tarefa tarefaEditada = new("Versão Final", PrioridadeTarefa.Normal)
        {
            Id = tarefaId,
            UsuarioId = usuarioId,
            DataCriacao = new DateOnly(2026, 11, 05)
        };

        tarefaEditada.AdicionarItem(new ItemTarefa(tarefaEditada, "Item novo") { UsuarioId = usuarioId, Concluido = true });

        // Ação
        bool conseguiuEditar = repositorioTarefa.Editar(tarefaId, tarefaEditada);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefaId);

        // Asserção
        Assert.IsTrue(conseguiuEditar);
        Assert.IsNotNull(tarefaSelecionada);
        Assert.AreEqual("Versão Final", tarefaSelecionada.Titulo);
        Assert.AreEqual(PrioridadeTarefa.Normal, tarefaSelecionada.Prioridade);
        Assert.AreEqual(new DateOnly(2026, 11, 05), tarefaSelecionada.DataCriacao);
        Assert.HasCount(1, tarefaSelecionada.Itens);
        Assert.IsTrue(tarefaSelecionada.Concluida);
        Assert.IsNotNull(tarefaSelecionada.DataConclusao);
    }

    [TestMethod]
    public void Excluir_DeveApagar_TarefaEItensNoBanco()
    {
        // Arranjo
        Guid tarefaId = Guid.CreateVersion7();

        Tarefa tarefaOriginal = new("Preparar Apresentação", PrioridadeTarefa.Alta)
        {
            Id = tarefaId,
            UsuarioId = usuarioId,
            DataCriacao = new DateOnly(2026, 11, 03)
        };

        tarefaOriginal.AdicionarItem(new ItemTarefa(tarefaOriginal, "Revisar conteúdo") { UsuarioId = usuarioId });

        RepositorioTarefaEmOrm repositorioTarefa = new(dbContext);

        repositorioTarefa.Cadastrar(tarefaOriginal);
        dbContext.ChangeTracker.Clear();

        // Ação
        bool conseguiuExcluir = repositorioTarefa.Excluir(tarefaId);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefaId);
        int quantidadeItensTarefa = dbContext.ItensTarefa.Count(i => i.TarefaId == tarefaId);

        // Asserção
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(tarefaSelecionada);
        Assert.AreEqual(0, quantidadeItensTarefa);


    }
}