using Domain.Entities.Finance;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Finance;

internal sealed class AccountHouseholdConfiguration : IEntityTypeConfiguration<AccountHousehold>
{
    public void Configure(EntityTypeBuilder<AccountHousehold> builder)
    {
        builder.ToTable("account_households", Schemas.Finance);

        builder.HasKey(ah => ah.AccountHouseholdId);

        builder.Property(ah => ah.AccountHouseholdId)
            .HasColumnName("account_household_id");

        builder.Property(ah => ah.AccountId)
            .HasColumnName("account_id")
            .IsRequired();

        builder.Property(ah => ah.HouseholdId)
            .HasColumnName("household_id")
            .IsRequired();

        // Auditing
        builder.ConfigureAuditable();

        // An account can be shared into a household only once
        builder.HasIndex(ah => new { ah.AccountId, ah.HouseholdId })
            .IsUnique();

        builder.HasIndex(ah => ah.HouseholdId);

        // Relations
        builder.HasOne(ah => ah.Household)
            .WithMany()
            .HasForeignKey(ah => ah.HouseholdId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
