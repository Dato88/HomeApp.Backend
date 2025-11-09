using SharedKernel;

namespace Domain.Entities.Recipes.Ref;

public class Allergen : AuditableEntity
{
    public int AllergenId { get; set; }
    public string Name { get; set; } = null!;
    public string? Code { get; set; }

    public ICollection<IngredientAllergen> IngredientAllergens { get; set; } = new List<IngredientAllergen>();
}
