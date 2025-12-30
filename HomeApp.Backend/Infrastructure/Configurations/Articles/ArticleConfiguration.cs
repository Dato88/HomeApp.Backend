using Domain.Entities.Articles;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Articles;

internal sealed class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("articles", Schemas.Article);

        builder.HasKey(a => a.ArticleId);

        builder.Property(a => a.ArticleId)
            .HasColumnName("article_id");

        builder.Property(a => a.ProductId)
            .HasColumnName("product_id")
            .IsRequired();

        builder.Property(a => a.ManufacturerId)
            .HasColumnName("manufacturer_id")
            .IsRequired();

        builder.Property(a => a.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.ConfigureAuditable();

        // Indexes
        builder.HasIndex(a => a.Name);
        builder.HasIndex(a => a.ProductId);
        builder.HasIndex(a => a.ManufacturerId);

        // Prevent duplicates per Product + Manufacturer
        builder.HasIndex(a => new { a.ProductId, a.ManufacturerId })
            .IsUnique();

        // Relations
        builder.HasOne(a => a.Product)
            .WithMany(p => p.Articles)
            .HasForeignKey(a => a.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Manufacturer)
            .WithMany(m => m.Articles)
            .HasForeignKey(a => a.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
