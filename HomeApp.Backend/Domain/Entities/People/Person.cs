using Domain.Entities.Households;
using Domain.Entities.Recipes;
using Domain.Entities.Todos;
using SharedKernel;

namespace Domain.Entities.People;

public class Person : AuditableEntity
{
    public int PersonId { get; set; }

    public string? Username { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string UserId { get; set; } = default!;

    public virtual ICollection<HouseholdMember> HouseholdMembers { get; set; } = new HashSet<HouseholdMember>();
    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public virtual ICollection<TodoPerson> TodoPeople { get; set; } = new HashSet<TodoPerson>();
    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
    public virtual ICollection<RecipeRating> RecipeRatings { get; set; } = new List<RecipeRating>();
    public virtual ICollection<RecipeComment> RecipeComments { get; set; } = new List<RecipeComment>();
}
