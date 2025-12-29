using Domain.Entities.Recipes;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes;

internal sealed class PopularSearchQueryConfiguration : IEntityTypeConfiguration<PopularSearchQuery>
{
    public void Configure(EntityTypeBuilder<PopularSearchQuery> builder)
    {
        builder.ToTable("popular_search_queries", Schemas.Recipe);

        builder.HasKey(psq => psq.QueryId);

        builder.Property(psq => psq.QueryId)
            .HasColumnName("query_id");

        builder.Property(psq => psq.QueryText)
            .HasColumnName("query_text")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(psq => psq.SearchCount)
            .HasColumnName("search_count");

        builder.Property(psq => psq.LastSearchedAt)
            .HasColumnName("last_searched_at");

        builder.ConfigureAuditable();

        builder.HasIndex(psq => psq.QueryText)
            .IsUnique();
    }
}
