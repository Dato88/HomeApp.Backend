using Domain.Entities.Budgets;
using Domain.Entities.Todos;

namespace Domain.Entities.People;

public class Person
{
    public int PersonId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserId { get; set; }

    public virtual ICollection<Budget> Budgets { get; set; } = new HashSet<Budget>();
    public virtual ICollection<TodoPerson> TodoPeople { get; set; } = new HashSet<TodoPerson>();
}
