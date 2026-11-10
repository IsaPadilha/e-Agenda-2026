using eAgenda.Dominio.Modulos.Tarefas;
using eAgenda.Infraestrutura.Compartilhado.Identity;
using eAgenda.Infraestrutura.Compartilhado.Orm;
using eAgenda.Infraestrutura.Modulos.Tarefas;
using eAgenda.Testes.Integracao.Compartilhado;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace eAgenda.Testes.Integracao;

[TestClass]
public sealed partial class RepositorioTarefaEmOrmTests
{
    private static string? connectionString = null;
    private Guid usuarioId = Guid.Empty;
    private EAgendaDbContext dbContext = null!;
    private RepositorioTarefaEmOrm repositorioTarefa = null!;

    [ClassInitialize]
    public static void InicializarClasse(TestContext testContext)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddUserSecrets("adac42fa-e092-4ba9-8660-66e04d303b3c")
            .Build();

        connectionString = configuration.GetConnectionString("SqlServer");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("A ConnectionStrings:SqlServer não foi configurada.");
    }

    [TestInitialize]
    public void InicializarTeste()
    {
        usuarioId = Guid.CreateVersion7();
        dbContext = CriarContexto(usuarioId);
        repositorioTarefa = new RepositorioTarefaEmOrm(dbContext);

        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        CriarUsuario(dbContext, usuarioId);
    }

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

        // Ação

        // Asserção
    }

    [TestMethod]
    public void Excluir_DeveApagar_TarefaEItensNoBanco()
    {
        // Arranjo

        // Ação

        // Asserção
    }

    private static EAgendaDbContext CriarContexto(Guid usuarioId)
    {
        DbContextOptions<EAgendaDbContext> options = new DbContextOptionsBuilder<EAgendaDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new EAgendaDbContext(options, new ProvedorUsuarioAtualFake(usuarioId));
    }

    private static void CriarUsuario(EAgendaDbContext dbContext, Guid usuarioId)
    {
        dbContext.Users.Add(new UsuarioIdentity
        {
            Id = usuarioId,
            UserName = $"integracao-{usuarioId:N}",
            NormalizedUserName = $"INTEGRACAO-{usuarioId:N}",
            Email = $"{usuarioId:N}@test.local",
            NormalizedEmail = $"{usuarioId:N}@TEST.LOCAL",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        });

        dbContext.SaveChanges();
    }
}