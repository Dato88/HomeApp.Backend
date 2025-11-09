using Domain.Entities.People;
using SharedKernel;

namespace Domain.Entities.Recipes.RecipesCore;

public class RecipeRating : AuditableEntity
{
    public int RecipeId { get; set; }
    public int PersonId { get; set; }
    public short Rating { get; set; }
    public DateTime RatedAt { get; set; }

    public Recipe Recipe { get; set; } = null!;
    public Person Person { get; set; } = null!;
}
