using SharedKernel;

namespace Domain.Entities.Recipes.Core;

public class RecipeStep : AuditableEntity
{
    public int RecipeStepId { get; set; }
    public int RecipeId { get; set; }
    public int StepNumber { get; set; }
    public string Instruction { get; set; } = null!;

    public Recipe Recipe { get; set; } = null!;
}
