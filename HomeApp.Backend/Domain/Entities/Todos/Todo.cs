using Domain.Entities.Todos.Enums;
using SharedKernel.ValueObjects;

namespace Domain.Entities.Todos;

public class Todo
{
    public TodoId TodoId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Name { get; set; }
    public bool Done { get; set; }
    public TodoPriority Priority { get; set; }
    public DateTime LastModified { get; set; } = DateTime.UtcNow;

    public virtual TodoGroupTodo TodoGroupTodo { get; set; }
    public virtual ICollection<TodoPerson> TodoPeople { get; set; } = new HashSet<TodoPerson>();
}
