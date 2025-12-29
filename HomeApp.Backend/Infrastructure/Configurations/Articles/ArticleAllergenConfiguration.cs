using Domain.Entities.Articles;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Articles;

internal sealed class ArticleAllergenConfiguration : IEntityTypeConfiguration<ArticleAllergen>
{
    public void Configure(EntityTypeBuilder<ArticleAllergen> builder)
    {
        builder.ToTable("article_allergens", Schemas.Article);

        builder.HasKey(x => new { x.ArticleId, x.AllergenId });

        builder.Property(x => x.ArticleId)
            .HasColumnName("article_id");

        builder.Property(x => x.AllergenId)
            .HasColumnName("allergen_id");

        builder.ConfigureAuditable();

        builder.HasIndex(x => x.ArticleId);
        builder.HasIndex(x => x.AllergenId);

        builder.HasOne(x => x.Article)
            .WithMany(a => a.ArticleAllergens)
            .HasForeignKey(x => x.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Allergen)
            .WithMany(a => a.ArticleAllergens)
            .HasForeignKey(x => x.AllergenId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
