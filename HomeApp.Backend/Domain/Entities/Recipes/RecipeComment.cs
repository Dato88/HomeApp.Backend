using Domain.Entities.People;
using SharedKernel;

namespace Domain.Entities.Recipes;

public class RecipeComment : AuditableEntity
{
    public int RecipeCommentId { get; set; }
    public int RecipeId { get; set; }
    public int PersonId { get; set; }
    public string CommentText { get; set; } = null!;

    public Recipe Recipe { get; set; } = null!;
    public Person Person { get; set; } = null!;
}
