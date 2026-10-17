using eAgenda.Dominio.Modulos.Contatos;
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

        builder.Property(c => c.Email)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(c => c.Telefone)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.Cargo)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(c => c.Empresa)
            .HasMaxLength(100)
            .IsRequired(false);

        // Configuração do relacionamento Um-para-Muitos com Compromisso (se aplicável)
        //builder.HasMany(c => c.Compromissos)
        //.WithOne(cp => cp.Contato) // Ajuste para o nome da propriedade de navegação na classe Compromisso, se houver
        //.HasForeignKey(cp => cp.ContatoId)
        //.OnDelete(DeleteBehavior.Restrict);
    }
}