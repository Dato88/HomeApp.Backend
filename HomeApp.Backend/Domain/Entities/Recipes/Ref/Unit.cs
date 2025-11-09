using Domain.Entities.Recipes.Core;
using Domain.Entities.Recipes.Pricing;
using SharedKernel;

namespace Domain.Entities.Recipes.Ref;

public class Unit : AuditableEntity
{
    public int UnitId { get; set; }
    public string Name { get; set; } = null!;
    public string Abbreviation { get; set; } = null!;

    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    public ICollection<IngredientPrice> IngredientPrices { get; set; } = new List<IngredientPrice>();
}
