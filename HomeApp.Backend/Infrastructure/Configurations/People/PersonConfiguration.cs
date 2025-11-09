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
    }
}
