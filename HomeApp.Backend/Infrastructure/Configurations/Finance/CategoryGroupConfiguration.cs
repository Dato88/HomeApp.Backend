using Domain.Entities.Finance;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Finance;

internal sealed class CategoryGroupConfiguration : IEntityTypeConfiguration<CategoryGroup>
{
    public void Configure(EntityTypeBuilder<CategoryGroup> builder)
    {
        builder.ToTable("category_groups", Schemas.Finance);

        builder.HasKey(g => g.CategoryGroupId);

        builder.Property(g => g.CategoryGroupId)
            .HasColumnName("category_group_id");

        builder.Property(g => g.HouseholdId)
            .HasColumnName("household_id")
            .IsRequired();

        builder.Property(g => g.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(g => g.CategoryGroupType)
            .HasColumnName("category_group_type")
            .IsRequired();

        builder.Property(g => g.TargetPercent)
            .HasColumnName("target_percent")
            .HasPrecision(5, 2);

        // Auditing
        builder.ConfigureAuditable();

        // Unique group names per household
        builder.HasIndex(g => new { g.HouseholdId, g.Name })
            .IsUnique();

        // Relations
        builder.HasOne(g => g.Household)
            .WithMany()
            .HasForeignKey(g => g.HouseholdId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
