using Domain.Entities.Budgets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Budgets;

public class BudgetCellConfiguration : IEntityTypeConfiguration<BudgetCell>
{
    public void Configure(EntityTypeBuilder<BudgetCell> builder)
    {
        builder.ToTable("BudgetCells");

        builder.HasKey(bc => bc.Id);

        builder.Property(bc => bc.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder.Property(bc => bc.BudgetRowId)
            .IsRequired();
        builder.Property(bc => bc.BudgetColumnId)
            .IsRequired();
        builder.Property(bc => bc.BudgetGroupId)
            .IsRequired();
        builder.Property(bc => bc.PersonId)
            .IsRequired();
        builder.Property(bc => bc.Year)
            .IsRequired();
        builder.Property(bc => bc.Name)
            .IsRequired();

        // Indices
        builder.HasIndex(bc => bc.BudgetRowId);
        builder.HasIndex(bc => bc.BudgetColumnId);
        builder.HasIndex(bc => bc.BudgetGroupId);
        builder.HasIndex(bc => bc.PersonId);
        builder.HasIndex(bc => bc.Year);

        // Relations
        builder.HasOne(bc => bc.BudgetRow)
            .WithMany(bc => bc.BudgetCells)
            .HasForeignKey(br => br.BudgetRowId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(bc => bc.BudgetColumn)
            .WithMany(bc => bc.BudgetCells)
            .HasForeignKey(bc => bc.BudgetColumnId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(bg => bg.BudgetGroup)
            .WithMany(bc => bc.BudgetCells)
            .HasForeignKey(bg => bg.BudgetGroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Person)
            .WithMany(bc => bc.BudgetCells)
            .HasForeignKey(p => p.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
