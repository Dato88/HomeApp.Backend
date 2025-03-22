using Domain.Budgets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Budgets;

public class BudgetGroupConfiguration : IEntityTypeConfiguration<BudgetGroup>
{
    public void Configure(EntityTypeBuilder<BudgetGroup> builder)
    {
        builder.ToTable("BudgetGroups");

        builder.HasKey(bg => bg.Id);

        builder.Property(bg => bg.CreatedAt).HasDefaultValue(DateTime.UtcNow);

        builder.Property(bg => bg.PersonId).IsRequired();
        builder.Property(bg => bg.Index).IsRequired();

        builder.Property(bg => bg.Name)
            .IsRequired()
            .HasMaxLength(150);

        // Indices
        builder.HasIndex(bg => bg.PersonId);
        builder.HasIndex(bg => bg.Index);

        // Relations
        builder.HasOne(p => p.Person)
            .WithMany(bg => bg.BudgetGroups)
            .HasForeignKey(p => p.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(bc => bc.BudgetCells)
            .WithOne(bg => bg.BudgetGroup)
            .HasForeignKey(bg => bg.BudgetGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
