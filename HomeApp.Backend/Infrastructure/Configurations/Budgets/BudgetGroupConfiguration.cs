using Domain.Entities.Budgets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Budgets;

internal sealed class BudgetGroupConfiguration : IEntityTypeConfiguration<BudgetGroup>
{
    public void Configure(EntityTypeBuilder<BudgetGroup> builder)
    {
        builder.ToTable("BudgetGroups");

        builder.HasKey(g => g.BudgetGroupId);

        builder.Property(g => g.BudgetId)
            .IsRequired();
        builder.Property(g => g.Index)
            .IsRequired();
        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(150);
        builder.Property(g => g.BudgetGroupType)
            .IsRequired();

        // Auditing
        builder.Property(g => g.CreatedAt)
            .HasDefaultValueSql("NOW()");
        builder.Property(g => g.CreatedById)
            .IsRequired();
        builder.Property(g => g.UpdatedAt);
        builder.Property(g => g.UpdatedById);

        // Indices (ordering unique within a budget)
        builder.HasIndex(g => g.BudgetId);
        builder.HasIndex(g => new { g.BudgetId, g.Index }).IsUnique();

        // Relations
        builder.HasOne(g => g.Budget)
            .WithMany(b => b.BudgetGroups)
            .HasForeignKey(g => g.BudgetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.BudgetRows)
            .WithOne(r => r.Group)
            .HasForeignKey(r => r.BudgetGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
