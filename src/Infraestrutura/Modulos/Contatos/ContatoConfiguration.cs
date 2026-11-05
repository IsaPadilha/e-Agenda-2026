using eAgenda.Dominio.Modulos.Contatos;
using eAgenda.Infraestrutura.Compartilhado.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAgenda.Infraestrutura.Modulos.Contatos;

public sealed class ContatoConfiguration : IEntityTypeConfiguration<Contato>
{
    public void Configure(EntityTypeBuilder<Contato> builder)
    {
        builder.ToTable("TBContatos");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.Property(c => c.Nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(c => new { c.UsuarioId, c.Email })
            .IsUnique();

        builder.HasIndex(c => new { c.UsuarioId, c.Telefone })
            .IsUnique();

        builder.Property(c => c.Cargo)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(c => c.Empresa)
            .HasMaxLength(100)
            .IsRequired(false);

        //Configuração do relacionamento Um-para-Muitos com Compromisso (se aplicável)
        builder.HasMany(c => c.Compromissos)
        .WithOne(cp => cp.Contato)
        .HasForeignKey(cp => cp.ContatoId)
        .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<UsuarioIdentity>()
            .WithMany()
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}