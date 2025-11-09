using SharedKernel;

namespace Domain.Entities.Todos;

public class TodoGroup : AuditableEntity
{
    public int TodoGroupId { get; set; }

    public string Title { get; set; } = default!;

    public virtual ICollection<TodoGroupTodo> TodoGroupTodos { get; set; } = new HashSet<TodoGroupTodo>();
}
