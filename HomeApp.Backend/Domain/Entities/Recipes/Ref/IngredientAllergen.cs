using SharedKernel;

namespace Domain.Entities.Recipes.Ref;

public class IngredientAllergen : AuditableEntity
{
    public int IngredientId { get; set; }
    public int AllergenId { get; set; }

    public Ingredient Ingredient { get; set; } = null!;
    public Allergen Allergen { get; set; } = null!;
}
