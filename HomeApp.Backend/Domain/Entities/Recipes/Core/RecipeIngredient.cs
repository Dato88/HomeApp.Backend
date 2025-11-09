using Domain.Entities.Recipes.Ref;
using SharedKernel;

namespace Domain.Entities.Recipes.Core;

public class RecipeIngredient : AuditableEntity
{
    public int RecipeIngredientId { get; set; }
    public int RecipeId { get; set; }
    public int IngredientId { get; set; }
    public int? UnitId { get; set; }
    public decimal? Quantity { get; set; }
    public string? Note { get; set; }
    public int SortOrder { get; set; }

    public Recipe Recipe { get; set; } = null!;
    public Ingredient Ingredient { get; set; } = null!;
    public Unit? Unit { get; set; }
}
