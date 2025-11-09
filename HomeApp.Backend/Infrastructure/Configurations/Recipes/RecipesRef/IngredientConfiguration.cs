using Domain.Entities.Recipes.RecipesNutrition;
using Domain.Entities.Recipes.RecipesRef;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes.RecipesRef;

public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        builder.ToTable("ingredients", Schemas.RecipesRef);

        builder.HasKey(x => x.IngredientId);

        builder.Property(x => x.IngredientId)
            .HasColumnName("ingredient_id");

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.IngredientCategoryId)
            .HasColumnName("ingredient_category_id");

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Ingredients)
            .HasForeignKey(x => x.IngredientCategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Nutrition)
            .WithOne(x => x.Ingredient)
            .HasForeignKey<IngredientNutrition>(x => x.IngredientId);

        builder.ConfigureAuditable();
    }
}
