using Domain.Entities.Budgets;
using Domain.Entities.Todos;
using SharedKernel.ValueObjects;

namespace Domain.Entities.People;

public class Person
{
    public PersonId PersonId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserId { get; set; }

    public virtual ICollection<BudgetCell> BudgetCells { get; set; } = new HashSet<BudgetCell>();
    public virtual ICollection<BudgetGroup> BudgetGroups { get; set; } = new HashSet<BudgetGroup>();
    public virtual ICollection<BudgetRow> BudgetRows { get; set; } = new HashSet<BudgetRow>();
    public virtual ICollection<TodoPerson> TodoPeople { get; set; } = new HashSet<TodoPerson>();
}
