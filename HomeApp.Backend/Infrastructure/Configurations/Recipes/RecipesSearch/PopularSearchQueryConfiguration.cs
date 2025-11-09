using Domain.Entities.Recipes.RecipesSearch;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes.RecipesSearch;

public class PopularSearchQueryConfiguration : IEntityTypeConfiguration<PopularSearchQuery>
{
    public void Configure(EntityTypeBuilder<PopularSearchQuery> builder)
    {
        builder.ToTable("popular_search_queries", Schemas.RecipesSearch);

        builder.HasKey(x => x.QueryId);

        builder.Property(x => x.QueryId)
            .HasColumnName("query_id");

        builder.Property(x => x.QueryText)
            .HasColumnName("query_text")
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(x => x.SearchCount)
            .HasColumnName("search_count")
            .HasDefaultValue(1)
            .IsRequired();

        builder.Property(x => x.LastSearchedAt)
            .HasColumnName("last_searched_at")
            .HasColumnType("timestamp(3) with time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.ConfigureAuditable();
    }
}
