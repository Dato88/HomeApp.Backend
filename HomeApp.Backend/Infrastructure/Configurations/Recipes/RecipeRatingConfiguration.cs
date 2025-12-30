using Domain.Entities.Recipes;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes;

internal sealed class RecipeRatingConfiguration : IEntityTypeConfiguration<RecipeRating>
{
    public void Configure(EntityTypeBuilder<RecipeRating> builder)
    {
        builder.ToTable("recipe_ratings", Schemas.Recipe);

        builder.HasKey(rr => new { rr.RecipeId, rr.PersonId });

        builder.Property(rr => rr.RecipeId)
            .HasColumnName("recipe_id");

        builder.Property(rr => rr.PersonId)
            .HasColumnName("person_id");

        builder.Property(rr => rr.Rating)
            .HasColumnName("rating");

        builder.Property(rr => rr.RatedAt)
            .HasColumnName("rated_at");

        builder.ConfigureAuditable();

        builder.HasOne(r => r.Recipe)
            .WithMany(r => r.Ratings)
            .HasForeignKey(r => r.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Person)
            .WithMany(p => p.RecipeRatings)
            .HasForeignKey(p => p.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
