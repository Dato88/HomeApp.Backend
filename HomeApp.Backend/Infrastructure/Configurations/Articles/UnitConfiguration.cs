using Domain.Entities.Articles;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Articles;

internal sealed class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.ToTable("units", Schemas.Article);

        builder.HasKey(u => u.UnitId);

        builder.Property(u => u.UnitId)
            .HasColumnName("unit_id");

        builder.Property(u => u.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Abbreviation)
            .HasColumnName("abbreviation")
            .IsRequired()
            .HasMaxLength(10);

        builder.ConfigureAuditable();

        builder.HasIndex(u => u.Abbreviation)
            .IsUnique();
    }
}
