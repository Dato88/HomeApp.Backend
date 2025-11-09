using Domain.Entities.Recipes.Ref;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes.Ref;

public class IngredientCategoryConfiguration : IEntityTypeConfiguration<IngredientCategory>
{
    public void Configure(EntityTypeBuilder<IngredientCategory> builder)
    {
        builder.ToTable("ingredient_categories", "ref");

        builder.HasKey(x => x.IngredientCategoryId);

        builder.Property(x => x.IngredientCategoryId)
            .HasColumnName("ingredient_category_id");

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.ParentCategoryId)
            .HasColumnName("parent_category_id");

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasOne(x => x.ParentCategory)
            .WithMany(x => x.Children)
            .HasForeignKey(x => x.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ConfigureAuditable();
    }
}
