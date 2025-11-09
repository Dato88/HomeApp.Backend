using Domain.Entities.Recipes.RecipesCore;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes.RecipesCore;

public class RecipeTagConfiguration : IEntityTypeConfiguration<RecipeTag>
{
    public void Configure(EntityTypeBuilder<RecipeTag> builder)
    {
        builder.ToTable("recipe_tags", Schemas.RecipesCore);

        builder.HasKey(x => new { x.RecipeId, x.TagId });

        builder.Property(x => x.RecipeId)
            .HasColumnName("recipe_id");

        builder.Property(x => x.TagId)
            .HasColumnName("tag_id");

        builder.HasOne(x => x.Recipe)
            .WithMany(x => x.RecipeTags)
            .HasForeignKey(x => x.RecipeId);

        builder.HasOne(x => x.Tag)
            .WithMany(x => x.RecipeTags)
            .HasForeignKey(x => x.TagId);

        builder.ConfigureAuditable();
    }
}
