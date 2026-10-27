using eAgenda.Dominio.Modulos.Categorias;
using eAgenda.Infraestrutura.Modulos.Categorias;
using eAgenda.Dominio.Modulos.Contatos;
using eAgenda.Infraestrutura.Modulos.Contatos;
using eAgenda.Dominio.Modulos.Tarefas;
using Microsoft.EntityFrameworkCore;
using eAgenda.Dominio.Modulos.Despesas;
using eAgenda.Infraestrutura.Modulos.Despesas;
using eAgenda.Infraestrutura.Modulos.Tarefas;
namespace eAgenda.Infraestrutura.Compartilhado.Orm;

using eAgenda.Dominio.Modulos.Contatos;

public sealed class EAgendaDbContext(DbContextOptions<EAgendaDbContext> options) : DbContext(options)
{
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Despesa> Despesas => Set<Despesa>();
    public DbSet<Tarefa> Tarefas => Set<Tarefa>();
    public DbSet<ItemTarefa> ItensTarefa => Set<ItemTarefa>();
    public DbSet<Contato> Contatos { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoriaConfiguration());
        modelBuilder.ApplyConfiguration(new DespesaConfiguration());
        modelBuilder.ApplyConfiguration(new TarefaConfiguration());
        modelBuilder.ApplyConfiguration(new ItemTarefaConfiguration());
        modelBuilder.ApplyConfiguration(new ContatoConfiguration());
    }
}