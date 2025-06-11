using SharedKernel.ValueObjects;

namespace Domain.Entities.Todos;

public class TodoGroup
{
    public TodoGroupId TodoGroupId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Name { get; set; }

    public virtual ICollection<TodoGroupTodo> TodoGroupTodos { get; set; } = new HashSet<TodoGroupTodo>();
}
