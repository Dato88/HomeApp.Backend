using Domain.Entities.Todos.Enums;
using SharedKernel;

namespace Domain.Entities.Todos;

public class Todo : IAudited
{
    public int TodoId { get; set; }

    public string Name { get; set; } = default!;
    public bool Done { get; set; }
    public TodoPriority Priority { get; set; }

    public virtual TodoGroupTodo TodoGroupTodo { get; set; }
    public virtual ICollection<TodoPerson> TodoPeople { get; set; } = new HashSet<TodoPerson>();

    public DateTime CreatedAt { get; set; }
    public int CreatedById { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedById { get; set; }
}
