using Domain.Entities.Recipes.RecipesCore;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes.RecipesCore;

public class RecipeImageConfiguration : IEntityTypeConfiguration<RecipeImage>
{
    public void Configure(EntityTypeBuilder<RecipeImage> builder)
    {
        builder.ToTable("recipe_images", Schemas.RecipesCore);

        builder.HasKey(x => x.RecipeImageId);

        builder.Property(x => x.RecipeImageId)
            .HasColumnName("recipe_image_id");

        builder.Property(x => x.RecipeId)
            .HasColumnName("recipe_id")
            .IsRequired();

        builder.Property(x => x.ImageUrl)
            .HasColumnName("image_url")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.IsPrimary)
            .HasColumnName("is_primary")
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasOne(x => x.Recipe)
            .WithMany(x => x.Images)
            .HasForeignKey(x => x.RecipeId);

        builder.ConfigureAuditable();
    }
}
