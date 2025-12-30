using Domain.Entities.Recipes;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes;

internal sealed class RecipeImageConfiguration : IEntityTypeConfiguration<RecipeImage>
{
    public void Configure(EntityTypeBuilder<RecipeImage> builder)
    {
        builder.ToTable("recipe_images", Schemas.Recipe);

        builder.HasKey(ri => ri.RecipeImageId);

        builder.Property(ri => ri.RecipeImageId)
            .HasColumnName("recipe_image_id");

        builder.Property(ri => ri.RecipeId)
            .HasColumnName("recipe_id");

        builder.Property(ri => ri.ImageUrl)
            .HasColumnName("image_url")
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(ri => ri.IsPrimary)
            .HasColumnName("is_primary")
            .HasDefaultValue(false);

        builder.ConfigureAuditable();

        builder.HasIndex(ri => ri.RecipeId);

        builder.HasOne(r => r.Recipe)
            .WithMany(r => r.Images)
            .HasForeignKey(r => r.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
