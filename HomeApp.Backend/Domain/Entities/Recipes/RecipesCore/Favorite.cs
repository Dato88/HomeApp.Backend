using Domain.Entities.People;
using SharedKernel;

namespace Domain.Entities.Recipes.RecipesCore;

public class Favorite : AuditableEntity
{
    public int PersonId { get; set; }
    public int RecipeId { get; set; }

    public Person Person { get; set; } = null!;
    public Recipe Recipe { get; set; } = null!;
}
