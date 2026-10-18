using eAgenda.Dominio.Modulos.Compromissos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAgenda.Infraestrutura.Modulos.Compromissos;

public sealed class CompromissoConfiguration : IEntityTypeConfiguration<Compromisso>
{
    public void Configure(EntityTypeBuilder<Compromisso> builder)
    {
        builder.ToTable("TBCompromissos");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.Property(c => c.Assunto)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.DataOcorrencia)
            .IsRequired();

        builder.Property(c => c.HoraInicio)
            .IsRequired();

        builder.Property(c => c.HoraTermino)
            .IsRequired();

        builder.Property(c => c.TipoCompromisso)
            .IsRequired();

        builder.Property(c => c.LocalOuLink)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(c => c.ContatoId)
            .IsRequired(false);

        // Configuração do relacionamento com Contato (Opcional)
        builder.HasOne(c => c.Contato)
            .WithMany(ct => ct.Compromissos)
            .HasForeignKey(c => c.ContatoId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }
}