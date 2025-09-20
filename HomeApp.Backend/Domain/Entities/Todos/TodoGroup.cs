using SharedKernel;

namespace Domain.Entities.Todos;

public class TodoGroup : IAudited
{
    public int TodoGroupId { get; set; }

    public string Name { get; set; } = default!;

    public virtual ICollection<TodoGroupTodo> TodoGroupTodos { get; set; } = new HashSet<TodoGroupTodo>();

    public DateTime CreatedAt { get; set; }
    public int CreatedById { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedById { get; set; }
}
