using SharedKernel;

namespace Domain.Entities.Recipes.Ref;

public class IngredientCategory : AuditableEntity
{
    public int IngredientCategoryId { get; set; }
    public string Name { get; set; } = null!;
    public int? ParentCategoryId { get; set; }

    public IngredientCategory? ParentCategory { get; set; }
    public ICollection<IngredientCategory> Children { get; set; } = new List<IngredientCategory>();
    public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
}
