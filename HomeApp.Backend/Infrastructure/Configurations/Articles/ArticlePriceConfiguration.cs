using Domain.Entities.Articles;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Articles;

internal sealed class ArticlePriceConfiguration : IEntityTypeConfiguration<ArticlePrice>
{
    public void Configure(EntityTypeBuilder<ArticlePrice> builder)
    {
        builder.ToTable("article_prices", Schemas.Article);

        builder.HasKey(p => p.ArticlePriceId);

        builder.Property(p => p.ArticlePriceId)
            .HasColumnName("article_price_id");

        builder.Property(p => p.ArticleId)
            .HasColumnName("article_id");

        builder.Property(p => p.StoreId)
            .HasColumnName("store_id");

        builder.Property(p => p.UnitId)
            .HasColumnName("unit_id");

        builder.Property(p => p.Price)
            .HasColumnName("price")
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(p => p.Currency)
            .HasColumnName("currency")
            .HasMaxLength(3)
            .HasDefaultValue("EUR");

        builder.Property(p => p.ValidFrom)
            .HasColumnName("valid_from")
            .IsRequired();

        builder.Property(p => p.ValidTo)
            .HasColumnName("valid_to");

        builder.ConfigureAuditable();

        builder.HasIndex(p => new { p.ArticleId, p.StoreId, p.UnitId, p.ValidFrom }).IsUnique();

        builder.HasIndex(p => p.ArticleId);
        builder.HasIndex(p => p.StoreId);
        builder.HasIndex(p => p.UnitId);

        builder.HasOne(p => p.Article)
            .WithMany(a => a.ArticlePrices)
            .HasForeignKey(p => p.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Store)
            .WithMany(s => s.ArticlePrices)
            .HasForeignKey(p => p.StoreId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Unit)
            .WithMany()
            .HasForeignKey(p => p.UnitId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
