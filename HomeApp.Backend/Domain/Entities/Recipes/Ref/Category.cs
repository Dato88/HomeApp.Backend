using Domain.Entities.Recipes.Core;
using SharedKernel;

namespace Domain.Entities.Recipes.Ref;

public class Category : AuditableEntity
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<RecipeCategory> RecipeCategories { get; set; } = new List<RecipeCategory>();
}
