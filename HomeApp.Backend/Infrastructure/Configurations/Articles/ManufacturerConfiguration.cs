using Domain.Entities.Articles;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Articles;

internal sealed class ManufacturerConfiguration : IEntityTypeConfiguration<Manufacturer>
{
    public void Configure(EntityTypeBuilder<Manufacturer> builder)
    {
        builder.ToTable("manufacturers", Schemas.Article);

        builder.HasKey(m => m.ManufacturerId);

        builder.Property(m => m.ManufacturerId)
            .HasColumnName("manufacturer_id");

        builder.Property(m => m.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.WebsiteUrl)
            .HasColumnName("website_url")
            .HasMaxLength(500);

        builder.ConfigureAuditable();

        builder.HasIndex(m => m.Name)
            .IsUnique();
    }
}
