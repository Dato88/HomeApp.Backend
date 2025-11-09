using Domain.Entities.Todos.Enums;
using SharedKernel;

namespace Domain.Entities.Todos;

public class Todo : AuditableEntity
{
    public int TodoId { get; set; }

    public string Title { get; set; } = default!;
    public bool Done { get; set; }
    public TodoPriority Priority { get; set; }

    public virtual TodoGroupTodo? TodoGroupTodo { get; set; }
    public virtual ICollection<TodoPerson> TodoPeople { get; set; } = new HashSet<TodoPerson>();
}
