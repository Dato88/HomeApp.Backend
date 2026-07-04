using Domain.Entities.Finance;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Finance;

internal sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("accounts", Schemas.Finance);

        builder.HasKey(a => a.AccountId);

        builder.Property(a => a.AccountId)
            .HasColumnName("account_id");

        builder.Property(a => a.PersonId)
            .HasColumnName("person_id")
            .IsRequired();

        builder.Property(a => a.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(a => a.Iban)
            .HasColumnName("iban")
            .HasMaxLength(34);

        builder.Property(a => a.Bic)
            .HasColumnName("bic")
            .HasMaxLength(11);

        builder.Property(a => a.AccountType)
            .HasColumnName("account_type")
            .IsRequired();

        builder.Property(a => a.CurrencyCode)
            .HasColumnName("currency_code")
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(a => a.Description)
            .HasColumnName("description")
            .HasMaxLength(500);

        builder.Property(a => a.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        // Auditing
        builder.ConfigureAuditable();

        // Indices
        builder.HasIndex(a => a.PersonId);

        builder.HasIndex(a => new { a.PersonId, a.Iban })
            .IsUnique()
            .HasFilter("iban IS NOT NULL");

        // Relations
        builder.HasOne(a => a.Person)
            .WithMany()
            .HasForeignKey(a => a.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.AccountHouseholds)
            .WithOne(ah => ah.Account)
            .HasForeignKey(ah => ah.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Transactions)
            .WithOne(t => t.Account)
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
