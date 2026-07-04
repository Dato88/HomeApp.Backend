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

        builder.Property(b => b.HouseholdId)
            .HasColumnName("household_id")
            .IsRequired();

        builder.Property(b => b.Year)
            .HasColumnName("year")
            .IsRequired();

        // Auditing
        builder.ConfigureAuditable();

        // One budget per household/year
        builder.HasIndex(b => new { b.HouseholdId, b.Year })
            .IsUnique();

        // Relations
        builder.HasOne(b => b.Household)
            .WithMany(h => h.Budgets)
            .HasForeignKey(b => b.HouseholdId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(b => b.BudgetGroups)
            .WithOne(g => g.Budget)
            .HasForeignKey(g => g.BudgetId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
