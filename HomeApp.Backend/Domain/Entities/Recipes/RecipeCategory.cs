using SharedKernel;

namespace Domain.Entities.Recipes;

public class RecipeCategory : AuditableEntity
{
    public int RecipeCategoryId { get; set; }
    public int RecipeId { get; set; }

    public string Name { get; set; } = null!;

    public Recipe Recipe { get; set; } = null!;
}
