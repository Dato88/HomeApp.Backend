using Domain.Todos.Enums;
using SharedKernel;

namespace Domain.Todos;

public class Todo : BaseClass
{
    public string Name { get; set; }
    public bool Done { get; set; }
    public TodoPriority Priority { get; set; }
    public DateTime LastModified { get; set; } = DateTime.UtcNow;

    public virtual TodoGroupTodo TodoGroupTodo { get; set; }
    public virtual ICollection<TodoPerson> TodoPeople { get; set; } = new HashSet<TodoPerson>();
}
