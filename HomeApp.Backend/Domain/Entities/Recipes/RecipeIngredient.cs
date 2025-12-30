using Domain.Entities.Articles;
using SharedKernel;

namespace Domain.Entities.Recipes;

public class RecipeIngredient : AuditableEntity
{
    public int RecipeIngredientId { get; set; }

    public int RecipeId { get; set; }

    public int ProductId { get; set; }

    public int? UnitId { get; set; }
    public decimal? Quantity { get; set; }

    public string? Note { get; set; }

    public int SortOrder { get; set; }

    public Product Product { get; set; } = null!;
    public Recipe Recipe { get; set; } = null!;
    public Unit? Unit { get; set; }
}
