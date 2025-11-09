using Domain.Entities.Recipes.RecipesCore;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes.RecipesCore;

public class RecipeStepConfiguration : IEntityTypeConfiguration<RecipeStep>
{
    public void Configure(EntityTypeBuilder<RecipeStep> builder)
    {
        builder.ToTable("recipe_steps", Schemas.RecipesCore);

        builder.HasKey(x => x.RecipeStepId);

        builder.Property(x => x.RecipeStepId)
            .HasColumnName("recipe_step_id");

        builder.Property(x => x.RecipeId)
            .HasColumnName("recipe_id")
            .IsRequired();

        builder.Property(x => x.StepNumber)
            .HasColumnName("step_number")
            .IsRequired();

        builder.Property(x => x.Instruction)
            .HasColumnName("instruction")
            .IsRequired();

        builder.HasIndex(x => new { x.RecipeId, x.StepNumber })
            .IsUnique();

        builder.HasOne(x => x.Recipe)
            .WithMany(x => x.Steps)
            .HasForeignKey(x => x.RecipeId);

        builder.ConfigureAuditable();
    }
}
