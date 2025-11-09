using Domain.Entities.Recipes.RecipesCore;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes.RecipesCore;

public class RecipeCategoryConfiguration : IEntityTypeConfiguration<RecipeCategory>
{
    public void Configure(EntityTypeBuilder<RecipeCategory> builder)
    {
        builder.ToTable("recipe_categories", Schemas.RecipesCore);

        builder.HasKey(x => new { x.RecipeId, x.CategoryId });

        builder.Property(x => x.RecipeId)
            .HasColumnName("recipe_id");

        builder.Property(x => x.CategoryId)
            .HasColumnName("category_id");

        builder.HasOne(x => x.Recipe)
            .WithMany(x => x.RecipeCategories)
            .HasForeignKey(x => x.RecipeId);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.RecipeCategories)
            .HasForeignKey(x => x.CategoryId);

        builder.ConfigureAuditable();
    }
}
