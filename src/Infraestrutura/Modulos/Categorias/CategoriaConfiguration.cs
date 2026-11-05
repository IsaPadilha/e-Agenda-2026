using eAgenda.Dominio.Modulos.Categorias;
using eAgenda.Infraestrutura.Compartilhado.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAgenda.Infraestrutura.Modulos.Categorias;

public sealed class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("TBCategorias");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.Property(c => c.Titulo)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(c => new { c.UsuarioId, c.Titulo })
            .IsUnique();

        builder.HasOne<UsuarioIdentity>()
            .WithMany()
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}