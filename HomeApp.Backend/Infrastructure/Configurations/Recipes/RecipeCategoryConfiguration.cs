using Domain.Entities.Recipes;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes;

internal sealed class RecipeCategoryConfiguration : IEntityTypeConfiguration<RecipeCategory>
{
    public void Configure(EntityTypeBuilder<RecipeCategory> builder)
    {
        builder.ToTable("recipe_categories", Schemas.Recipe);

        builder.HasKey(rc => rc.RecipeCategoryId);

        builder.Property(rc => rc.RecipeId)
            .HasColumnName("recipe_id");

        builder.Property(rc => rc.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(100);

        builder.ConfigureAuditable();

        builder.HasIndex(rc => new { rc.RecipeId, rc.Name })
            .IsUnique();

        builder.HasOne(rc => rc.Recipe)
            .WithMany(r => r.RecipeCategories)
            .HasForeignKey(rc => rc.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
