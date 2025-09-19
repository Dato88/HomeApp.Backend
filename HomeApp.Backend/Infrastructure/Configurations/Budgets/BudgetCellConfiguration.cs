using Domain.Entities.Budgets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Budgets;

internal sealed class BudgetCellConfiguration : IEntityTypeConfiguration<BudgetCell>
{
    public void Configure(EntityTypeBuilder<BudgetCell> builder)
    {
        builder.ToTable("BudgetCells");

        builder.HasKey(c => c.BudgetCellId);

        builder.Property(c => c.BudgetRowId)
            .IsRequired();
        builder.Property(c => c.Month)
            .IsRequired();
        builder.Property(c => c.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        // Auditing
        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("NOW()");
        builder.Property(c => c.CreatedById)
            .IsRequired();
        builder.Property(c => c.UpdatedAt);
        builder.Property(c => c.UpdatedById)
            .HasDefaultValue(0);

        // Indices / Constraints
        builder.HasIndex(c => c.BudgetRowId);
        builder.HasIndex(c => new { c.BudgetRowId, c.Month })
            .IsUnique();

        builder.ToTable(t => t.HasCheckConstraint(
            "ck_budgetcell_month",
            "\"Month\" BETWEEN 1 AND 12"
        ));

        // Relations
        builder.HasOne(c => c.BudgetRow)
            .WithMany(r => r.Cells)
            .HasForeignKey(c => c.BudgetRowId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
