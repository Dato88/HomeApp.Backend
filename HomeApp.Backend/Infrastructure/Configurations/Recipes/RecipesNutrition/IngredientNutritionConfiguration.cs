using Domain.Entities.Recipes.RecipesNutrition;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes.RecipesNutrition;

public class IngredientNutritionConfiguration : IEntityTypeConfiguration<IngredientNutrition>
{
    public void Configure(EntityTypeBuilder<IngredientNutrition> builder)
    {
        builder.ToTable("ingredient_nutrition", Schemas.RecipesNutrition);

        builder.HasKey(x => x.IngredientId);

        builder.Property(x => x.IngredientId)
            .HasColumnName("ingredient_id");

        builder.Property(x => x.CaloriesKcalPer100g)
            .HasColumnName("calories_kcal_per_100g")
            .HasColumnType("numeric(8,2)");

        builder.Property(x => x.ProteinGPer100g)
            .HasColumnName("protein_g_per_100g")
            .HasColumnType("numeric(8,2)");

        builder.Property(x => x.CarbsGPer100g)
            .HasColumnName("carbs_g_per_100g")
            .HasColumnType("numeric(8,2)");

        builder.Property(x => x.SugarGPer100g)
            .HasColumnName("sugar_g_per_100g")
            .HasColumnType("numeric(8,2)");

        builder.Property(x => x.FatGPer100g)
            .HasColumnName("fat_g_per_100g")
            .HasColumnType("numeric(8,2)");

        builder.Property(x => x.SaturatedFatGPer100g)
            .HasColumnName("saturated_fat_g_per_100g")
            .HasColumnType("numeric(8,2)");

        builder.Property(x => x.FiberGPer100g)
            .HasColumnName("fiber_g_per_100g")
            .HasColumnType("numeric(8,2)");

        builder.Property(x => x.SaltGPer100g)
            .HasColumnName("salt_g_per_100g")
            .HasColumnType("numeric(8,2)");

        builder.Property(x => x.LastSource)
            .HasColumnName("last_source")
            .HasMaxLength(200);

        builder.HasOne(x => x.Ingredient)
            .WithOne(x => x.Nutrition)
            .HasForeignKey<IngredientNutrition>(x => x.IngredientId);

        builder.ConfigureAuditable();
    }
}
