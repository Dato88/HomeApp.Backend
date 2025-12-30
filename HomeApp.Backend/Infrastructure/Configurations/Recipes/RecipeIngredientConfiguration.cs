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
            .HasColumnName("recipe_id")
            .IsRequired();

        builder.Property(ri => ri.ProductId)
            .HasColumnName("product_id")
            .IsRequired();

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

        // Indexes
        builder.HasIndex(ri => ri.ProductId);
        builder.HasIndex(ri => ri.RecipeId);

        builder.HasIndex(ri => new { ri.RecipeId, ri.SortOrder })
            .IsUnique();

        // Relations
        builder.HasOne(p => p.Product)
            .WithMany()
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Recipe)
            .WithMany(r => r.Ingredients)
            .HasForeignKey(r => r.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.Unit)
            .WithMany()
            .HasForeignKey(u => u.UnitId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
