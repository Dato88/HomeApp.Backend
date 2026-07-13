using Domain.Entities.Finance;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Finance;

internal sealed class PaymentPartnerConfiguration : IEntityTypeConfiguration<PaymentPartner>
{
    public void Configure(EntityTypeBuilder<PaymentPartner> builder)
    {
        builder.ToTable("payment_partners", Schemas.Finance);

        builder.HasKey(p => p.PaymentPartnerId);

        builder.Property(p => p.PaymentPartnerId)
            .HasColumnName("payment_partner_id");

        builder.Property(p => p.PersonId)
            .HasColumnName("person_id")
            .IsRequired();

        builder.Property(p => p.DisplayName)
            .HasColumnName("display_name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.NormalizedName)
            .HasColumnName("normalized_name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Iban)
            .HasColumnName("iban")
            .HasMaxLength(34);

        builder.Property(p => p.LinkedAccountId)
            .HasColumnName("linked_account_id");

        // Auditing
        builder.ConfigureAuditable();

        // Indices: when an IBAN is present it is the partner's identity; without one the
        // normalized name is. The same name may still exist on several IBAN-bearing partners.
        // Named HasIndex overloads: two separate indexes over the same column pair.
        builder.HasIndex(p => new { p.PersonId, p.Iban })
            .IsUnique()
            .HasFilter("iban IS NOT NULL")
            .HasDatabaseName("ix_payment_partners_person_id_iban");

        builder.HasIndex(p => new { p.PersonId, p.NormalizedName },
                "ix_payment_partners_person_id_normalized_name_no_iban")
            .IsUnique()
            .HasFilter("iban IS NULL")
            .HasDatabaseName("ix_payment_partners_person_id_normalized_name_no_iban");

        builder.HasIndex(p => new { p.PersonId, p.NormalizedName },
                "ix_payment_partners_person_id_normalized_name")
            .HasDatabaseName("ix_payment_partners_person_id_normalized_name");

        builder.HasIndex(p => p.LinkedAccountId);

        // Relations
        builder.HasOne(p => p.Person)
            .WithMany()
            .HasForeignKey(p => p.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.LinkedAccount)
            .WithMany()
            .HasForeignKey(p => p.LinkedAccountId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
