using Domain.Entities.Recipes;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes;

internal sealed class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("tags", Schemas.Recipe);

        builder.HasKey(t => t.TagId);

        builder.Property(t => t.TagId)
            .HasColumnName("tag_id");

        builder.Property(t => t.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(50);

        builder.ConfigureAuditable();

        builder.HasIndex(t => t.Name)
            .IsUnique();
    }
}
