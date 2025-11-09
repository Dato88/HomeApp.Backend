using Domain.Entities.Recipes.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes.Core;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.ToTable("recipes", "core");

        builder.HasKey(x => x.RecipeId);

        builder.Property(x => x.RecipeId)
            .HasColumnName("recipe_id");

        builder.Property(x => x.PersonId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(x => x.Title)
            .HasColumnName("title")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("description");

        builder.Property(x => x.PrepTimeMinutes)
            .HasColumnName("prep_time_minutes");

        builder.Property(x => x.CookTimeMinutes)
            .HasColumnName("cook_time_minutes");

        builder.Property(x => x.Servings)
            .HasColumnName("servings")
            .HasColumnType("numeric(5,2)");

        builder.Property(x => x.IsPublic)
            .HasColumnName("is_public")
            .HasDefaultValue(true)
            .IsRequired();

        builder.HasOne(x => x.Person)
            .WithMany(x => x.Recipes)
            .HasForeignKey(x => x.PersonId);

        builder.ConfigureAuditable();
    }
}
