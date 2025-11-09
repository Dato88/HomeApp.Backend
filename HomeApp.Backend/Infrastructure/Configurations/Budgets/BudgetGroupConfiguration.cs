using Domain.Entities.Budgets;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Budgets;

internal sealed class BudgetGroupConfiguration : IEntityTypeConfiguration<BudgetGroup>
{
    public void Configure(EntityTypeBuilder<BudgetGroup> builder)
    {
        builder.ToTable("budget_groups", Schemas.Budget);

        builder.HasKey(g => g.BudgetGroupId);

        builder.Property(g => g.BudgetGroupId)
            .HasColumnName("budget_group_id");

        builder.Property(g => g.BudgetId)
            .HasColumnName("budget_id")
            .IsRequired();

        builder.Property(g => g.Index)
            .HasColumnName("index")
            .IsRequired();

        builder.Property(g => g.Title)
            .HasColumnName("title")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(g => g.BudgetGroupType)
            .HasColumnName("budget_group_type")
            .IsRequired();

        // Auditing
        builder.ConfigureAuditable();

        // Indices (ordering unique within a budget)
        builder.HasIndex(g => g.BudgetId);
        builder.HasIndex(g => new { g.BudgetId, g.Index }).IsUnique();

        // Relations
        builder.HasOne(g => g.Budget)
            .WithMany(b => b.BudgetGroups)
            .HasForeignKey(g => g.BudgetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.BudgetRows)
            .WithOne(r => r.BudgetGroup)
            .HasForeignKey(r => r.BudgetGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
