using Domain.Entities.Recipes.RecipesCore;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes.RecipesCore;

public class RecipeIngredientConfiguration : IEntityTypeConfiguration<RecipeIngredient>
{
    public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
    {
        builder.ToTable("recipe_ingredients", Schemas.RecipesCore);

        builder.HasKey(x => x.RecipeIngredientId);

        builder.Property(x => x.RecipeIngredientId)
            .HasColumnName("recipe_ingredient_id");

        builder.Property(x => x.RecipeId)
            .HasColumnName("recipe_id")
            .IsRequired();

        builder.Property(x => x.IngredientId)
            .HasColumnName("ingredient_id")
            .IsRequired();

        builder.Property(x => x.UnitId)
            .HasColumnName("unit_id");

        builder.Property(x => x.Quantity)
            .HasColumnName("quantity")
            .HasColumnType("numeric(10,2)");

        builder.Property(x => x.Note)
            .HasColumnName("note")
            .HasMaxLength(200);

        builder.Property(x => x.SortOrder)
            .HasColumnName("sort_order")
            .HasDefaultValue(1)
            .IsRequired();

        builder.HasIndex(x => x.RecipeId);

        builder.HasOne(x => x.Recipe)
            .WithMany(x => x.Ingredients)
            .HasForeignKey(x => x.RecipeId);

        builder.HasOne(x => x.Ingredient)
            .WithMany(x => x.RecipeIngredients)
            .HasForeignKey(x => x.IngredientId);

        builder.HasOne(x => x.Unit)
            .WithMany(x => x.RecipeIngredients)
            .HasForeignKey(x => x.UnitId);

        builder.ConfigureAuditable();
    }
}
