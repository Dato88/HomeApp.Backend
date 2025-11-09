using Domain.Entities.Recipes.RecipesCore;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes.RecipesCore;

public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.ToTable("favorites", Schemas.RecipesCore);

        builder.HasKey(x => new { x.PersonId, x.RecipeId });

        builder.Property(x => x.PersonId)
            .HasColumnName("user_id");

        builder.Property(x => x.RecipeId)
            .HasColumnName("recipe_id");

        builder.HasOne(x => x.Person)
            .WithMany(x => x.Favorites)
            .HasForeignKey(x => x.PersonId);

        builder.HasOne(x => x.Recipe)
            .WithMany(x => x.Favorites)
            .HasForeignKey(x => x.RecipeId);

        builder.ConfigureAuditable();
    }
}
