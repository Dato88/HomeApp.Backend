using Domain.Entities.Budgets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Budgets;

internal sealed class BudgetRowConfiguration : IEntityTypeConfiguration<BudgetRow>
{
    public void Configure(EntityTypeBuilder<BudgetRow> builder)
    {
        builder.ToTable("BudgetRows");

        builder.HasKey(r => r.BudgetRowId);

        builder.Property(r => r.BudgetGroupId)
            .IsRequired();
        builder.Property(r => r.Index)
            .IsRequired();
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(150);

        // Auditing
        builder.Property(r => r.CreatedAt)
            .HasDefaultValueSql("NOW()");
        builder.Property(r => r.CreatedById)
            .IsRequired();
        builder.Property(r => r.UpdatedAt);
        builder.Property(r => r.UpdatedById)
            .HasDefaultValue(0);

        // Indices (ordering unique within a group)
        builder.HasIndex(r => r.BudgetGroupId);
        builder.HasIndex(r => new { r.BudgetGroupId, r.Index })
            .IsUnique();

        // Relations
        builder.HasOne(r => r.Group)
            .WithMany(g => g.BudgetRows)
            .HasForeignKey(r => r.BudgetGroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.Cells)
            .WithOne(c => c.BudgetRow)
            .HasForeignKey(c => c.BudgetRowId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
