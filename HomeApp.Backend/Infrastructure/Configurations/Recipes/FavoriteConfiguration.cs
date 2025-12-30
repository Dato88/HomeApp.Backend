using Domain.Entities.Recipes;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes;

internal sealed class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.ToTable("recipe_favorites", Schemas.Recipe);

        builder.HasKey(f => new { f.PersonId, f.RecipeId });

        builder.Property(f => f.PersonId)
            .HasColumnName("person_id");

        builder.Property(f => f.RecipeId)
            .HasColumnName("recipe_id");

        builder.ConfigureAuditable();

        builder.HasOne(p => p.Person)
            .WithMany(p => p.Favorites)
            .HasForeignKey(p => p.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Recipe)
            .WithMany(r => r.Favorites)
            .HasForeignKey(r => r.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
