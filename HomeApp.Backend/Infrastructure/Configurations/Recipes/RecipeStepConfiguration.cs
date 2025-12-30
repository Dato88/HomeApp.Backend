using Domain.Entities.Recipes;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recipes;

internal sealed class RecipeStepConfiguration : IEntityTypeConfiguration<RecipeStep>
{
    public void Configure(EntityTypeBuilder<RecipeStep> builder)
    {
        builder.ToTable("recipe_steps", Schemas.Recipe);

        builder.HasKey(rs => rs.RecipeStepId);

        builder.Property(rs => rs.RecipeStepId)
            .HasColumnName("recipe_step_id");

        builder.Property(rs => rs.RecipeId)
            .HasColumnName("recipe_id");

        builder.Property(rs => rs.StepNumber)
            .HasColumnName("step_number");

        builder.Property(rs => rs.Instruction)
            .HasColumnName("instruction")
            .IsRequired();

        builder.ConfigureAuditable();

        builder.HasIndex(rs => new { rs.RecipeId, rs.StepNumber })
            .IsUnique();

        builder.HasOne(r => r.Recipe)
            .WithMany(r => r.Steps)
            .HasForeignKey(r => r.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
