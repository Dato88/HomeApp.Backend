using Domain.Entities.Todos.Enums;

namespace Domain.Entities.Todos;

public class Todo
{
    public int TodoId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Name { get; set; }
    public bool Done { get; set; }
    public TodoPriority Priority { get; set; }
    public DateTime LastModified { get; set; } = DateTime.UtcNow;

    public virtual TodoGroupTodo TodoGroupTodo { get; set; }
    public virtual ICollection<TodoPerson> TodoPeople { get; set; } = new HashSet<TodoPerson>();
}
