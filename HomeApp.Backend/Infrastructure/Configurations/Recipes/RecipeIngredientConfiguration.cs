using Domain.Entities.Recipes;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes;

internal sealed class RecipeIngredientConfiguration : IEntityTypeConfiguration<RecipeIngredient>
{
    public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
    {
        builder.ToTable("recipe_ingredients", Schemas.Recipe);

        builder.HasKey(ri => ri.RecipeIngredientId);

        builder.Property(ri => ri.RecipeIngredientId)
            .HasColumnName("recipe_ingredient_id");

        builder.Property(ri => ri.RecipeId)
            .HasColumnName("recipe_id");

        builder.Property(ri => ri.ArticleId)
            .HasColumnName("article_id");

        builder.Property(ri => ri.UnitId)
            .HasColumnName("unit_id");

        builder.Property(ri => ri.Quantity)
            .HasColumnName("quantity");

        builder.Property(ri => ri.Note)
            .HasColumnName("note")
            .HasMaxLength(200);

        builder.Property(ri => ri.SortOrder)
            .HasColumnName("sort_order");

        builder.ConfigureAuditable();

        builder.HasIndex(ri => ri.RecipeId);
        builder.HasIndex(ri => ri.ArticleId);

        builder.HasIndex(ri => new { ri.RecipeId, ri.SortOrder })
            .IsUnique();

        builder.HasOne(ri => ri.Recipe)
            .WithMany(r => r.Ingredients)
            .HasForeignKey(ri => ri.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ri => ri.Article)
            .WithMany()
            .HasForeignKey(ri => ri.ArticleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ri => ri.Unit)
            .WithMany()
            .HasForeignKey(ri => ri.UnitId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
