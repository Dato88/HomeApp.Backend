using Domain.Entities.Recipes.RecipesCore;
using SharedKernel;

namespace Domain.Entities.Recipes.RecipesRef;

public class Tag : AuditableEntity
{
    public int TagId { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<RecipeTag> RecipeTags { get; set; } = new List<RecipeTag>();
}
