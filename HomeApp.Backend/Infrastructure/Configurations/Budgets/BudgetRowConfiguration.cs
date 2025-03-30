using Domain.Entities.Budgets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Budgets;

public class BudgetRowConfiguration : IEntityTypeConfiguration<BudgetRow>
{
    public void Configure(EntityTypeBuilder<BudgetRow> builder)
    {
        builder.ToTable("BudgetRows");

        builder.HasKey(br => br.Id);

        builder.Property(br => br.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder.Property(br => br.PersonId)
            .IsRequired();
        builder.Property(br => br.Index)
            .IsRequired();
        builder.Property(br => br.Year)
            .IsRequired();

        builder.Property(br => br.Name)
            .IsRequired()
            .HasMaxLength(150);

        // Indices
        builder.HasIndex(br => br.PersonId);
        builder.HasIndex(br => br.Year);
        builder.HasIndex(br => br.Index);

        // Relations
        builder.HasOne(p => p.Person)
            .WithMany(br => br.BudgetRows)
            .HasForeignKey(p => p.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(bc => bc.BudgetCells)
            .WithOne(br => br.BudgetRow)
            .HasForeignKey(br => br.BudgetRowId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
