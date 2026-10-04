using eAgenda.Dominio.Modulos.Categorias;
using eAgenda.Infraestrutura.Modulos.Categorias;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Infraestrutura.Compartilhado.Orm;

public sealed class EAgendaDbContext(DbContextOptions<EAgendaDbContext> options) : DbContext(options)
{
    public DbSet<Categoria> Categorias => Set<Categoria>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoriaConfiguration());
    }
}