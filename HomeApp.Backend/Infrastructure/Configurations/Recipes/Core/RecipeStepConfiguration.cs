using Domain.Entities.Recipes.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes.Core;

public class RecipeStepConfiguration : IEntityTypeConfiguration<RecipeStep>
{
    public void Configure(EntityTypeBuilder<RecipeStep> builder)
    {
        builder.ToTable("recipe_steps", "core");

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
