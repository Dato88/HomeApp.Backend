using Domain.Entities.Recipes.Ref;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes.Ref;

public class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.ToTable("units", "ref");

        builder.HasKey(x => x.UnitId);

        builder.Property(x => x.UnitId)
            .HasColumnName("unit_id");

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Abbreviation)
            .HasColumnName("abbreviation")
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasIndex(x => x.Abbreviation)
            .IsUnique();

        builder.ConfigureAuditable();
    }
}
