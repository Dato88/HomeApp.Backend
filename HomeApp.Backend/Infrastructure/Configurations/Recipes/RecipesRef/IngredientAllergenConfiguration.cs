using Domain.Entities.Recipes.RecipesRef;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes.RecipesRef;

public class IngredientAllergenConfiguration : IEntityTypeConfiguration<IngredientAllergen>
{
    public void Configure(EntityTypeBuilder<IngredientAllergen> builder)
    {
        builder.ToTable("ingredient_allergens", Schemas.RecipesRef);

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
