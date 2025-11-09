using Domain.Entities.Recipes.RecipesCore;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes.RecipesCore;

public class RecipeCommentConfiguration : IEntityTypeConfiguration<RecipeComment>
{
    public void Configure(EntityTypeBuilder<RecipeComment> builder)
    {
        builder.ToTable("recipe_comments", Schemas.RecipesCore);

        builder.HasKey(x => x.CommentId);

        builder.Property(x => x.CommentId)
            .HasColumnName("comment_id");

        builder.Property(x => x.RecipeId)
            .HasColumnName("recipe_id")
            .IsRequired();

        builder.Property(x => x.PersonId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(x => x.CommentText)
            .HasColumnName("comment_text")
            .IsRequired();

        builder.HasOne(x => x.Recipe)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.RecipeId);

        builder.HasOne(x => x.Person)
            .WithMany(x => x.RecipeComments)
            .HasForeignKey(x => x.PersonId);

        builder.ConfigureAuditable();
    }
}
