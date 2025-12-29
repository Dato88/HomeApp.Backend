using Domain.Entities.Recipes;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes;

internal sealed class RecipeCommentConfiguration : IEntityTypeConfiguration<RecipeComment>
{
    public void Configure(EntityTypeBuilder<RecipeComment> builder)
    {
        builder.ToTable("recipe_comments", Schemas.Recipe);

        builder.HasKey(rc => rc.RecipeCommentId);

        builder.Property(rc => rc.RecipeCommentId)
            .HasColumnName("recipe_comment_id");

        builder.Property(rc => rc.RecipeId)
            .HasColumnName("recipe_id");

        builder.Property(rc => rc.PersonId)
            .HasColumnName("person_id");

        builder.Property(rc => rc.CommentText)
            .HasColumnName("comment_text")
            .IsRequired();

        builder.ConfigureAuditable();

        builder.HasIndex(rc => rc.RecipeId);

        builder.HasOne(rc => rc.Recipe)
            .WithMany(r => r.Comments)
            .HasForeignKey(rc => rc.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rc => rc.Person)
            .WithMany()
            .HasForeignKey(rc => rc.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
