using Domain.Entities.Households;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Households;

internal sealed class HouseholdMemberConfiguration : IEntityTypeConfiguration<HouseholdMember>
{
    public void Configure(EntityTypeBuilder<HouseholdMember> builder)
    {
        builder.ToTable("household_members", Schemas.People);

        builder.HasKey(m => m.HouseholdMemberId);

        builder.Property(m => m.HouseholdMemberId)
            .HasColumnName("household_member_id");

        builder.Property(m => m.HouseholdId)
            .HasColumnName("household_id")
            .IsRequired();

        builder.Property(m => m.PersonId)
            .HasColumnName("person_id")
            .IsRequired();

        // Auditing
        builder.ConfigureAuditable();

        // A person can be member of many households, but only once per household
        builder.HasIndex(m => new { m.HouseholdId, m.PersonId })
            .IsUnique();

        builder.HasIndex(m => m.PersonId);

        // Relations
        builder.HasOne(m => m.Person)
            .WithMany(p => p.HouseholdMembers)
            .HasForeignKey(m => m.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
