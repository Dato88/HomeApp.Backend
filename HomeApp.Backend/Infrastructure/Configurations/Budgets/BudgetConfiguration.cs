using Domain.Entities.Budgets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Budgets;

internal sealed class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.ToTable("Budgets");

        builder.HasKey(b => b.BudgetId);

        builder.Property(b => b.PersonId)
            .IsRequired();
        builder.Property(b => b.Year)
            .IsRequired();

        // Auditing
        builder.Property(b => b.CreatedAt)
            .HasDefaultValueSql("NOW()");
        builder.Property(b => b.CreatedById)
            .IsRequired();
        builder.Property(b => b.UpdatedAt);
        builder.Property(b => b.UpdatedById);

        // One budget per person/year
        builder.HasIndex(b => new { b.PersonId, b.Year })
            .IsUnique();

        // Relations
        builder.HasOne(b => b.Person)
            .WithMany(p => p.Budgets)
            .HasForeignKey(b => b.PersonId)
            .HasPrincipalKey(p => p.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(b => b.BudgetGroups)
            .WithOne(g => g.Budget)
            .HasForeignKey(g => g.BudgetId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
