using Domain.Entities.Recipes.RecipesSearch;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes.RecipesSearch;

public class RecipeSearchIndexConfiguration : IEntityTypeConfiguration<RecipeSearchIndex>
{
    public void Configure(EntityTypeBuilder<RecipeSearchIndex> builder)
    {
        builder.ToTable("recipe_search_index", Schemas.RecipesSearch);

        builder.HasKey(x => x.RecipeId);

        builder.Property(x => x.RecipeId)
            .HasColumnName("recipe_id");

        builder.Property(x => x.Title)
            .HasColumnName("title")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Content)
            .HasColumnName("content")
            .IsRequired();

        builder.Property(x => x.LastIndexedAt)
            .HasColumnName("last_indexed_at")
            .HasColumnType("timestamp(3) with time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.HasOne(x => x.Recipe)
            .WithOne(x => x.SearchIndex)
            .HasForeignKey<RecipeSearchIndex>(x => x.RecipeId);

        builder.ConfigureAuditable();
    }
}
