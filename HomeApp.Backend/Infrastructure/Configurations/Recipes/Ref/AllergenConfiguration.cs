using Domain.Entities.Recipes.Ref;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes.Ref;

public class AllergenConfiguration : IEntityTypeConfiguration<Allergen>
{
    public void Configure(EntityTypeBuilder<Allergen> builder)
    {
        builder.ToTable("allergens", "ref");

        builder.HasKey(x => x.AllergenId);

        builder.Property(x => x.AllergenId)
            .HasColumnName("allergen_id");

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Code)
            .HasColumnName("code")
            .HasMaxLength(20);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.ConfigureAuditable();
    }
}
