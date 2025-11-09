using Domain.Entities.Recipes.Ref;
using SharedKernel;

namespace Domain.Entities.Recipes.Core;

public class RecipeTag : AuditableEntity
{
    public int RecipeId { get; set; }
    public int TagId { get; set; }

    public Recipe Recipe { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}
