using Domain.Entities.Budgets;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Budgets;

internal sealed class BudgetRowConfiguration : IEntityTypeConfiguration<BudgetRow>
{
    public void Configure(EntityTypeBuilder<BudgetRow> builder)
    {
        builder.ToTable("budget_rows", Schemas.Budget);

        builder.HasKey(r => r.BudgetRowId);

        builder.Property(r => r.BudgetRowId)
            .HasColumnName("budget_row_id");

        builder.Property(r => r.BudgetGroupId)
            .HasColumnName("budget_group_id")
            .IsRequired();

        builder.Property(r => r.Index)
            .HasColumnName("index")
            .IsRequired();

        builder.Property(r => r.Title)
            .HasColumnName("title")
            .IsRequired()
            .HasMaxLength(150);


        // Auditing
        builder.ConfigureAuditable();

        // Indices (ordering unique within a group)
        builder.HasIndex(r => r.BudgetGroupId);
        builder.HasIndex(r => new { r.BudgetGroupId, r.Index })
            .IsUnique();

        // Relations
        builder.HasOne(r => r.BudgetGroup)
            .WithMany(g => g.BudgetRows)
            .HasForeignKey(r => r.BudgetGroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.BudgetCells)
            .WithOne(c => c.BudgetRow)
            .HasForeignKey(c => c.BudgetRowId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
