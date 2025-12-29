using Domain.Entities.Articles;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Articles;

internal sealed class ArticleCategoryConfiguration : IEntityTypeConfiguration<ArticleCategory>
{
    public void Configure(EntityTypeBuilder<ArticleCategory> builder)
    {
        builder.ToTable("article_categories", Schemas.Article);

        builder.HasKey(c => c.ArticleCategoryId);

        builder.Property(c => c.ArticleCategoryId)
            .HasColumnName("article_category_id");

        builder.Property(c => c.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.ParentCategoryId)
            .HasColumnName("parent_category_id");

        builder.ConfigureAuditable();

        builder.HasIndex(c => c.Name);
        builder.HasIndex(c => c.ParentCategoryId);

        builder.HasOne(c => c.ParentCategory)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
