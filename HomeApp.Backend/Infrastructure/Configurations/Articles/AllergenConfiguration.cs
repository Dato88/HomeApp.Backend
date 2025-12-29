using Domain.Entities.Articles;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Articles;

internal sealed class AllergenConfiguration : IEntityTypeConfiguration<Allergen>
{
    public void Configure(EntityTypeBuilder<Allergen> builder)
    {
        builder.ToTable("allergens", Schemas.Article);

        builder.HasKey(a => a.AllergenId);

        builder.Property(a => a.AllergenId)
            .HasColumnName("allergen_id");

        builder.Property(a => a.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Code)
            .HasColumnName("code")
            .HasMaxLength(50);

        builder.ConfigureAuditable();

        builder.HasIndex(a => a.Name)
            .IsUnique();

        builder.HasIndex(a => a.Code);
    }
}
