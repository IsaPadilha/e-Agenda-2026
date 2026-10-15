using eAgenda.Dominio.Modulos.Tarefas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAgenda.Infraestrutura.Modulos.Tarefas;

public sealed class TarefaConfiguration : IEntityTypeConfiguration<Tarefa>
{
    public void Configure(EntityTypeBuilder<Tarefa> builder)
    {
        builder.ToTable("TBTarefas");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedNever();

        builder.Property(t => t.Titulo)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.Prioridade)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(t => t.DataCriacao)
            .HasColumnType("date")
            .IsRequired();
    }
}