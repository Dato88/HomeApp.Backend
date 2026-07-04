using Domain.Entities.People;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.People;

internal sealed class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("people", Schemas.People);

        builder.HasKey(p => p.PersonId);

        builder.Property(p => p.PersonId)
            .HasColumnName("person_id");

        builder.Property(p => p.Username)
            .HasColumnName("username")
            .HasMaxLength(150);

        builder.Property(p => p.FirstName)
            .HasColumnName("first_name")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.LastName)
            .HasColumnName("last_name")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.Email)
            .HasColumnName("email")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.UserId)
            .HasColumnName("user_id")
            .IsRequired()
            .HasMaxLength(36);

        // Auditing
        builder.ConfigureAuditable();

        // Indices
        builder.HasIndex(p => p.Email)
            .IsUnique();
        builder.HasIndex(p => p.UserId)
            .IsUnique();
        builder.HasIndex(p => p.Username)
            .IsUnique();

        // Relationen
        builder.HasMany(tp => tp.TodoPeople)
            .WithOne(p => p.Person)
            .HasForeignKey(p => p.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        // Recipes
        builder.HasMany(p => p.Recipes)
            .WithOne(r => r.Person)
            .HasForeignKey(r => r.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        // Favorites
        builder.HasMany(p => p.Favorites)
            .WithOne(f => f.Person)
            .HasForeignKey(f => f.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        // Recipe Ratings
        builder.HasMany(p => p.RecipeRatings)
            .WithOne(rr => rr.Person)
            .HasForeignKey(rr => rr.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        // Recipe Comments
        builder.HasMany(p => p.RecipeComments)
            .WithOne(rc => rc.Person)
            .HasForeignKey(rc => rc.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
