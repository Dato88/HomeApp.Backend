using Domain.Entities.Budgets;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Budgets;

internal sealed class BudgetCellConfiguration : IEntityTypeConfiguration<BudgetCell>
{
    public void Configure(EntityTypeBuilder<BudgetCell> builder)
    {
        builder.ToTable("budget_cells", Schemas.Budget);

        builder.HasKey(c => c.BudgetCellId);

        builder.Property(c => c.BudgetCellId)
            .HasColumnName("budget_cell_id");

        builder.Property(c => c.BudgetRowId)
            .HasColumnName("budget_row_id")
            .IsRequired();

        builder.Property(c => c.Month)
            .HasColumnName("month")
            .IsRequired();

        builder.Property(c => c.Amount)
            .HasColumnName("amount")
            .HasPrecision(18, 2)
            .IsRequired();

        // Auditing
        builder.ConfigureAuditable();

        // Indices / Constraints
        builder.HasIndex(c => c.BudgetRowId);
        builder.HasIndex(c => new { c.BudgetRowId, c.Month })
            .IsUnique();

        builder.ToTable(t => t.HasCheckConstraint(
            "ck_budgetcell_month",
            "month BETWEEN 1 AND 12"
        ));

        // Relations
        builder.HasOne(c => c.BudgetRow)
            .WithMany(r => r.BudgetCells)
            .HasForeignKey(c => c.BudgetRowId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
