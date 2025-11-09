using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel;

namespace Infrastructure.Configurations;

public static class AuditableEntityConfigurationExtensions
{
    public static void ConfigureAuditable<T>(this EntityTypeBuilder<T> builder)
        where T : AuditableEntity
    {
        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp(3) with time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(e => e.CreatedById)
            .HasColumnName("created_by_id")
            .IsRequired();
        ;

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp(3) with time zone");

        builder.Property(e => e.UpdatedById)
            .HasColumnName("updated_by_id");
    }
}
