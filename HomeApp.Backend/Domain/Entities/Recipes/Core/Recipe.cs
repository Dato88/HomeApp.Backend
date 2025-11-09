using Domain.Entities.People;
using Domain.Entities.Recipes.Search;
using SharedKernel;

namespace Domain.Entities.Recipes.Core;

public class Recipe : AuditableEntity
{
    public int RecipeId { get; set; }
    public int PersonId { get; set; }

    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int? PrepTimeMinutes { get; set; }
    public int? CookTimeMinutes { get; set; }
    public decimal? Servings { get; set; }
    public bool IsPublic { get; set; }

    public Person Person { get; set; } = null!;
    public ICollection<RecipeStep> Steps { get; set; } = new List<RecipeStep>();
    public ICollection<RecipeIngredient> Ingredients { get; set; } = new List<RecipeIngredient>();
    public ICollection<RecipeImage> Images { get; set; } = new List<RecipeImage>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public ICollection<RecipeRating> Ratings { get; set; } = new List<RecipeRating>();
    public ICollection<RecipeComment> Comments { get; set; } = new List<RecipeComment>();
    public ICollection<RecipeCategory> RecipeCategories { get; set; } = new List<RecipeCategory>();
    public ICollection<RecipeTag> RecipeTags { get; set; } = new List<RecipeTag>();
    public RecipeSearchIndex? SearchIndex { get; set; }
}
