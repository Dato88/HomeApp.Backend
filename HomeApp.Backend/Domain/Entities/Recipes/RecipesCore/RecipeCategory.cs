using Domain.Entities.Recipes.RecipesRef;
using SharedKernel;

namespace Domain.Entities.Recipes.RecipesCore;

public class RecipeCategory : AuditableEntity
{
    public int RecipeId { get; set; }
    public int CategoryId { get; set; }

    public Recipe Recipe { get; set; } = null!;
    public Category Category { get; set; } = null!;
}
