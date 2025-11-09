using Domain.Entities.Recipes.Ref;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes.Ref;

public class IngredientAllergenConfiguration : IEntityTypeConfiguration<IngredientAllergen>
{
    public void Configure(EntityTypeBuilder<IngredientAllergen> builder)
    {
        builder.ToTable("ingredient_allergens", "ref");

        builder.HasKey(x => new { x.IngredientId, x.AllergenId });

        builder.Property(x => x.IngredientId)
            .HasColumnName("ingredient_id");

        builder.Property(x => x.AllergenId)
            .HasColumnName("allergen_id");

        builder.HasOne(x => x.Ingredient)
            .WithMany(x => x.IngredientAllergens)
            .HasForeignKey(x => x.IngredientId);

        builder.HasOne(x => x.Allergen)
            .WithMany(x => x.IngredientAllergens)
            .HasForeignKey(x => x.AllergenId);

        builder.ConfigureAuditable();
    }
}
