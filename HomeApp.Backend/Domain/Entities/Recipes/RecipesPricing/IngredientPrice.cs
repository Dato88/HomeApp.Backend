using Domain.Entities.Recipes.RecipesRef;
using SharedKernel;

namespace Domain.Entities.Recipes.RecipesPricing;

public class IngredientPrice : AuditableEntity
{
    public int IngredientPriceId { get; set; }
    public int IngredientId { get; set; }
    public int StoreId { get; set; }
    public int UnitId { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "EUR";
    public DateTime ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }

    public Ingredient Ingredient { get; set; } = null!;
    public Store Store { get; set; } = null!;
    public Unit Unit { get; set; } = null!;
}
