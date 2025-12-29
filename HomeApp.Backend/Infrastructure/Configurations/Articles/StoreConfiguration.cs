using Domain.Entities.Articles;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Articles;

internal sealed class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.ToTable("stores", Schemas.Article);

        builder.HasKey(s => s.StoreId);

        builder.Property(s => s.StoreId)
            .HasColumnName("store_id");

        builder.Property(s => s.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(s => s.WebsiteUrl)
            .HasColumnName("website_url")
            .HasMaxLength(300);

        builder.ConfigureAuditable();

        builder.HasIndex(s => s.Name)
            .IsUnique();
    }
}
