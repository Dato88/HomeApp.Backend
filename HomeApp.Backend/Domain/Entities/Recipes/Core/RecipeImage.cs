using SharedKernel;

namespace Domain.Entities.Recipes.Core;

public class RecipeImage : AuditableEntity
{
    public int RecipeImageId { get; set; }
    public int RecipeId { get; set; }
    public string ImageUrl { get; set; } = null!;
    public bool IsPrimary { get; set; }

    public Recipe Recipe { get; set; } = null!;
}
