using eAgenda.Dominio.Modulos.Categorias;
using eAgenda.Infraestrutura.Modulos.Categorias;
using Microsoft.EntityFrameworkCore;
using eAgenda.Dominio.Modulos.Despesas;
using eAgenda.Infraestrutura.Modulos.Despesas;
namespace eAgenda.Infraestrutura.Compartilhado.Orm;

public sealed class EAgendaDbContext(DbContextOptions<EAgendaDbContext> options) : DbContext(options)
{
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Despesa> Despesas => Set<Despesa>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoriaConfiguration());
        modelBuilder.ApplyConfiguration(new DespesaConfiguration());
    }
}