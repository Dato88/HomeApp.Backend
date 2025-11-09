using Domain.Entities.Recipes.RecipesRef;
using SharedKernel;

namespace Domain.Entities.Recipes.RecipesNutrition;

public class IngredientNutrition : AuditableEntity
{
    public int IngredientId { get; set; }

    public decimal? CaloriesKcalPer100g { get; set; }
    public decimal? ProteinGPer100g { get; set; }
    public decimal? CarbsGPer100g { get; set; }
    public decimal? SugarGPer100g { get; set; }
    public decimal? FatGPer100g { get; set; }
    public decimal? SaturatedFatGPer100g { get; set; }
    public decimal? FiberGPer100g { get; set; }
    public decimal? SaltGPer100g { get; set; }
    public string? LastSource { get; set; }

    public Ingredient Ingredient { get; set; } = null!;
}
