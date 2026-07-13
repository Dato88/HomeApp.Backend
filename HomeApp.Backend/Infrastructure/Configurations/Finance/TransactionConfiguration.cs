using Domain.Entities.Finance;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Finance;

internal sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions", Schemas.Finance);

        builder.HasKey(t => t.TransactionId);

        builder.Property(t => t.TransactionId)
            .HasColumnName("transaction_id");

        builder.Property(t => t.AccountId)
            .HasColumnName("account_id")
            .IsRequired();

        builder.Property(t => t.BookingDate)
            .HasColumnName("booking_date")
            .IsRequired();

        builder.Property(t => t.ValueDate)
            .HasColumnName("value_date");

        builder.Property(t => t.Amount)
            .HasColumnName("amount")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(t => t.PaymentPartnerName)
            .HasColumnName("payment_partner_name")
            .HasMaxLength(200);

        builder.Property(t => t.PaymentPartnerIban)
            .HasColumnName("payment_partner_iban")
            .HasMaxLength(34);

        builder.Property(t => t.Purpose)
            .HasColumnName("purpose")
            .HasMaxLength(500);

        builder.Property(t => t.BankReference)
            .HasColumnName("bank_reference")
            .HasMaxLength(100);

        builder.Property(t => t.CategoryId)
            .HasColumnName("category_id");

        builder.Property(t => t.PaymentPartnerId)
            .HasColumnName("payment_partner_id");

        builder.Property(t => t.ImportHash)
            .HasColumnName("import_hash")
            .HasMaxLength(64);

        builder.Property(t => t.Source)
            .HasColumnName("source")
            .IsRequired();

        // Auditing
        builder.ConfigureAuditable();

        // Indices
        builder.HasIndex(t => new { t.AccountId, t.BookingDate });

        builder.HasIndex(t => t.CategoryId);

        builder.HasIndex(t => t.PaymentPartnerId);

        // Import dedup: the same import hash may exist only once per account
        builder.HasIndex(t => new { t.AccountId, t.ImportHash })
            .IsUnique()
            .HasFilter("import_hash IS NOT NULL");

        // Relations
        builder.HasOne(t => t.Category)
            .WithMany(c => c.Transactions)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(t => t.PaymentPartner)
            .WithMany(p => p.Transactions)
            .HasForeignKey(t => t.PaymentPartnerId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
