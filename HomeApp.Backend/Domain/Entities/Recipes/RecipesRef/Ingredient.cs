using Domain.Entities.Recipes.RecipesCore;
using Domain.Entities.Recipes.RecipesNutrition;
using Domain.Entities.Recipes.RecipesPricing;
using SharedKernel;

namespace Domain.Entities.Recipes.RecipesRef;

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
