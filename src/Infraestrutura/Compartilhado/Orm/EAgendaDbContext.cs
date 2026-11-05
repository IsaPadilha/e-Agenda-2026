using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.Modulos.Compromissos;
using eAgenda.Dominio.Modulos.Categorias;
using eAgenda.Infraestrutura.Modulos.Categorias;
using eAgenda.Dominio.Modulos.Tarefas;
using Microsoft.EntityFrameworkCore;
using eAgenda.Dominio.Modulos.Despesas;
using eAgenda.Infraestrutura.Modulos.Despesas;
using eAgenda.Infraestrutura.Modulos.Tarefas;
using eAgenda.Dominio.Modulos.Contatos;
using eAgenda.Infraestrutura.Compartilhado.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace eAgenda.Infraestrutura.Compartilhado.Orm;

public sealed class EAgendaDbContext(
    DbContextOptions<EAgendaDbContext> options,
    IProvedorUsuarioAtual usuarioAtual
) : IdentityDbContext<UsuarioIdentity, IdentityRole<Guid>, Guid>(options)
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

        // QueryFilter aplica um filtro global para isolar os dados de cada usuário
        modelBuilder.Entity<Categoria>().HasQueryFilter(c => c.UsuarioId == usuarioAtual.Id);
        modelBuilder.Entity<Despesa>().HasQueryFilter(c => c.UsuarioId == usuarioAtual.Id);
        modelBuilder.Entity<Tarefa>().HasQueryFilter(c => c.UsuarioId == usuarioAtual.Id);
        modelBuilder.Entity<ItemTarefa>().HasQueryFilter(c => c.UsuarioId == usuarioAtual.Id);
        modelBuilder.Entity<Contato>().HasQueryFilter(c => c.UsuarioId == usuarioAtual.Id);
        modelBuilder.Entity<Compromisso>().HasQueryFilter(c => c.UsuarioId == usuarioAtual.Id);
    }
}