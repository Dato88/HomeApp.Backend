using Domain.Entities.Recipes;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes;

internal sealed class RecipeTagConfiguration : IEntityTypeConfiguration<RecipeTag>
{
    public void Configure(EntityTypeBuilder<RecipeTag> builder)
    {
        builder.ToTable("recipe_tags", Schemas.Recipe);

        builder.HasKey(rt => new { rt.RecipeId, rt.TagId });

        builder.Property(rt => rt.RecipeId)
            .HasColumnName("recipe_id");

        builder.Property(rt => rt.TagId)
            .HasColumnName("tag_id");

        builder.ConfigureAuditable();

        builder.HasOne(r => r.Recipe)
            .WithMany(r => r.RecipeTags)
            .HasForeignKey(r => r.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.Tag)
            .WithMany(t => t.RecipeTags)
            .HasForeignKey(t => t.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
