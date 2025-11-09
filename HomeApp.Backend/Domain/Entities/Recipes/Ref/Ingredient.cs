using Domain.Entities.Recipes.Core;
using Domain.Entities.Recipes.Nutrition;
using Domain.Entities.Recipes.Pricing;
using SharedKernel;

namespace Domain.Entities.Recipes.Ref;

public class Ingredient : AuditableEntity
{
    public int IngredientId { get; set; }
    public string Name { get; set; } = null!;
    public int? IngredientCategoryId { get; set; }
    public bool IsActive { get; set; }

    public IngredientCategory? Category { get; set; }
    public IngredientNutrition? Nutrition { get; set; }
    public ICollection<IngredientAllergen> IngredientAllergens { get; set; } = new List<IngredientAllergen>();
    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    public ICollection<IngredientPrice> IngredientPrices { get; set; } = new List<IngredientPrice>();
}
