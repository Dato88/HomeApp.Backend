using SharedKernel;

namespace Domain.Entities.Recipes;

public class RecipeSearchIndex : AuditableEntity
{
    public int RecipeId { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime LastIndexedAt { get; set; }

    public Recipe Recipe { get; set; } = null!;
}
