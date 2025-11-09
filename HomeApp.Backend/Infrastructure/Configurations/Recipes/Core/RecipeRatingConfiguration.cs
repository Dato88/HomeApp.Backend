using Domain.Entities.Recipes.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes.Core;

public class RecipeRatingConfiguration : IEntityTypeConfiguration<RecipeRating>
{
    public void Configure(EntityTypeBuilder<RecipeRating> builder)
    {
        builder.ToTable("recipe_ratings", "core");

        builder.HasKey(x => new { x.RecipeId, x.PersonId });

        builder.Property(x => x.RecipeId)
            .HasColumnName("recipe_id");

        builder.Property(x => x.PersonId)
            .HasColumnName("user_id");

        builder.Property(x => x.Rating)
            .HasColumnName("rating")
            .IsRequired();

        builder.Property(x => x.RatedAt)
            .HasColumnName("rated_at")
            .HasColumnType("timestamp(3) with time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();
        builder.HasCheckConstraint("ck_recipe_ratings_rating", "rating BETWEEN 1 AND 5");

        builder.HasOne(x => x.Recipe)
            .WithMany(x => x.Ratings)
            .HasForeignKey(x => x.RecipeId);

        builder.HasOne(x => x.Person)
            .WithMany(x => x.RecipeRatings)
            .HasForeignKey(x => x.PersonId);

        builder.ConfigureAuditable();
    }
}
