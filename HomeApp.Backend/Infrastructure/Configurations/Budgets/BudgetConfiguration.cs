using Domain.Entities.Budgets;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Budgets;

internal sealed class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.ToTable("budgets", Schemas.Budget);

        builder.HasKey(b => b.BudgetId);

        builder.Property(b => b.BudgetId)
            .HasColumnName("budget_id");

        builder.Property(b => b.PersonId)
            .HasColumnName("person_id")
            .IsRequired();

        builder.Property(b => b.Year)
            .HasColumnName("year")
            .IsRequired();

        // Auditing
        builder.ConfigureAuditable();

        // One budget per person/year
        builder.HasIndex(b => new { b.PersonId, b.Year })
            .IsUnique();

        // Relations
        builder.HasOne(b => b.Person)
            .WithMany(p => p.Budgets)
            .HasForeignKey(b => b.PersonId)
            .HasPrincipalKey(p => p.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(b => b.BudgetGroups)
            .WithOne(g => g.Budget)
            .HasForeignKey(g => g.BudgetId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
