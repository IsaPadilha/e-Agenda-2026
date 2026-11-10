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