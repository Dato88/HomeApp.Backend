using Domain.Entities.Recipes.RecipesCore;
using Domain.Entities.Recipes.RecipesPricing;
using SharedKernel;

namespace Domain.Entities.Recipes.RecipesRef;

public class Unit : AuditableEntity
{
    public int UnitId { get; set; }
    public string Name { get; set; } = null!;
    public string Abbreviation { get; set; } = null!;

    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    public ICollection<IngredientPrice> IngredientPrices { get; set; } = new List<IngredientPrice>();
}
