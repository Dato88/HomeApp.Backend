using Domain.Entities.Articles;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Articles;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products", Schemas.Article);

        builder.HasKey(p => p.ProductId);

        builder.Property(p => p.ProductId)
            .HasColumnName("product_id");

        builder.Property(p => p.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasColumnName("description")
            .HasMaxLength(500);

        builder.Property(p => p.ArticleCategoryId)
            .HasColumnName("article_category_id");

        builder.ConfigureAuditable();

        builder.HasIndex(p => p.Name)
            .IsUnique();

        builder.HasIndex(p => p.ArticleCategoryId);

        builder.HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.ArticleCategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
