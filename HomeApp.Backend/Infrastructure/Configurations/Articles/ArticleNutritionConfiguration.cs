using Domain.Entities.Articles;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Articles;

internal sealed class ArticleNutritionConfiguration : IEntityTypeConfiguration<ArticleNutrition>
{
    public void Configure(EntityTypeBuilder<ArticleNutrition> builder)
    {
        builder.ToTable("article_nutritions", Schemas.Article);

        builder.HasKey(n => n.ArticleId);

        builder.Property(n => n.ArticleId)
            .HasColumnName("article_id");

        builder.Property(n => n.CaloriesKcalPer100g)
            .HasColumnName("calories_kcal_per_100g");

        builder.Property(n => n.ProteinGPer100g)
            .HasColumnName("protein_g_per_100g");

        builder.Property(n => n.CarbsGPer100g)
            .HasColumnName("carbs_g_per_100g");

        builder.Property(n => n.SugarGPer100g)
            .HasColumnName("sugar_g_per_100g");

        builder.Property(n => n.FatGPer100g)
            .HasColumnName("fat_g_per_100g");

        builder.Property(n => n.SaturatedFatGPer100g)
            .HasColumnName("saturated_fat_g_per_100g");

        builder.Property(n => n.FiberGPer100g)
            .HasColumnName("fiber_g_per_100g");

        builder.Property(n => n.SaltGPer100g)
            .HasColumnName("salt_g_per_100g");

        builder.Property(n => n.LastSource)
            .HasColumnName("last_source")
            .HasMaxLength(100);

        builder.ConfigureAuditable();

        builder.HasOne(n => n.Article)
            .WithOne(a => a.ArticleNutrition)
            .HasForeignKey<ArticleNutrition>(n => n.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
