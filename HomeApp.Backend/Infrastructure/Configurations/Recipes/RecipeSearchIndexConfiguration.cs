using Domain.Entities.Recipes;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes;

internal sealed class RecipeSearchIndexConfiguration : IEntityTypeConfiguration<RecipeSearchIndex>
{
    public void Configure(EntityTypeBuilder<RecipeSearchIndex> builder)
    {
        builder.ToTable("recipe_search_index", Schemas.Recipe);

        builder.HasKey(rsi => rsi.RecipeId);

        builder.Property(rsi => rsi.RecipeId)
            .HasColumnName("recipe_id");

        builder.Property(rsi => rsi.Title)
            .HasColumnName("title")
            .IsRequired();

        builder.Property(rsi => rsi.Content)
            .HasColumnName("content")
            .IsRequired();

        builder.Property(rsi => rsi.LastIndexedAt)
            .HasColumnName("last_indexed_at");

        builder.ConfigureAuditable();

        builder.HasOne(r => r.Recipe)
            .WithOne(r => r.SearchIndex)
            .HasForeignKey<RecipeSearchIndex>(r => r.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
