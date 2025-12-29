using SharedKernel;

namespace Domain.Entities.Recipes;

public class Tag : AuditableEntity
{
    public int TagId { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<RecipeTag> RecipeTags { get; set; } = new List<RecipeTag>();
}
