using Domain.Entities.Recipes.Core;
using SharedKernel;

namespace Domain.Entities.Recipes.Search;

public class RecipeSearchIndex : AuditableEntity
{
    public int RecipeId { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime LastIndexedAt { get; set; }

    public Recipe Recipe { get; set; } = null!;
}
