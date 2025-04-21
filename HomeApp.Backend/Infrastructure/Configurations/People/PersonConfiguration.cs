using Domain.Entities.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.People;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("People");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("NOW()");

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

        // Indices
        builder.HasIndex(p => p.Email)
            .IsUnique();
        builder.HasIndex(p => p.UserId)
            .IsUnique();

        // Relations
        builder.HasMany(tp => tp.TodoPeople)
            .WithOne(p => p.Person)
            .HasForeignKey(p => p.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(bc => bc.BudgetCells)
            .WithOne(p => p.Person)
            .HasForeignKey(p => p.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(bg => bg.BudgetGroups)
            .WithOne(p => p.Person)
            .HasForeignKey(p => p.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(br => br.BudgetRows)
            .WithOne(p => p.Person)
            .HasForeignKey(p => p.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
