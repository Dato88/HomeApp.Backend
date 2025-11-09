using Domain.Entities.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.People;

internal sealed class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("People");

        builder.HasKey(p => p.PersonId);

        builder.Property(p => p.Username)
            .HasMaxLength(150);

        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.UserId)
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
