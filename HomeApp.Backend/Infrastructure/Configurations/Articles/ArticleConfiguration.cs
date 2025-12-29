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

        builder.Property(a => a.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.ArticleCategoryId)
            .HasColumnName("article_category_id");

        builder.Property(a => a.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.ConfigureAuditable();

        builder.HasIndex(a => a.Name)
            .IsUnique();

        builder.HasIndex(a => a.ArticleCategoryId);

        builder.HasOne(a => a.Category)
            .WithMany(c => c.Articles)
            .HasForeignKey(a => a.ArticleCategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
