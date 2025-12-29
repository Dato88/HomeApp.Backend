using Domain.Entities.Recipes;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes;

internal sealed class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.ToTable("recipes", Schemas.Recipe);

        builder.HasKey(r => r.RecipeId);

        builder.Property(r => r.RecipeId)
            .HasColumnName("recipe_id");

        builder.Property(r => r.PersonId)
            .HasColumnName("person_id");

        builder.Property(r => r.Title)
            .HasColumnName("title")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(r => r.Description)
            .HasColumnName("description");

        builder.Property(r => r.PrepTimeMinutes)
            .HasColumnName("prep_time_minutes");

        builder.Property(r => r.CookTimeMinutes)
            .HasColumnName("cook_time_minutes");

        builder.Property(r => r.Servings)
            .HasColumnName("servings");

        builder.Property(r => r.IsPublic)
            .HasColumnName("is_public")
            .HasDefaultValue(false);

        builder.ConfigureAuditable();

        builder.HasIndex(r => r.PersonId);
        builder.HasIndex(r => r.IsPublic);

        builder.HasOne(r => r.Person)
            .WithMany()
            .HasForeignKey(r => r.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
