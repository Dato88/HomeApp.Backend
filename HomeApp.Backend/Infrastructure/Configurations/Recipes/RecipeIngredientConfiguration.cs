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

        builder.HasOne(r => r.Recipe)
            .WithMany(r => r.Ingredients)
            .HasForeignKey(r => r.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Article)
            .WithMany()
            .HasForeignKey(a => a.ArticleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Unit)
            .WithMany()
            .HasForeignKey(u => u.UnitId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
