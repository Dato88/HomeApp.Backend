using Domain.Budgets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Budgets;

public class BudgetColumnConfiguration : IEntityTypeConfiguration<BudgetColumn>
{
    public void Configure(EntityTypeBuilder<BudgetColumn> builder)
    {
        builder.ToTable("BudgetColumns");

        builder.HasKey(bc => bc.Id);

        builder.Property(bc => bc.CreatedAt).HasDefaultValue(DateTime.UtcNow);

        builder.Property(bc => bc.Index).IsRequired();

        builder.Property(bc => bc.Name)
            .IsRequired()
            .HasMaxLength(150);

        // Indices
        builder.HasIndex(bc => bc.Index);
        builder.HasIndex(bc => bc.Name);

        // Relations
        builder.HasMany(bc => bc.BudgetCells)
            .WithOne(bc => bc.BudgetColumn)
            .HasForeignKey(bc => bc.BudgetColumnId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
