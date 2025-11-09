using Domain.Entities.Recipes.RecipesCore;
using SharedKernel;

namespace Domain.Entities.Recipes.RecipesRef;

public class Category : AuditableEntity
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<RecipeCategory> RecipeCategories { get; set; } = new List<RecipeCategory>();
}
