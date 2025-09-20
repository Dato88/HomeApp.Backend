using SharedKernel;

namespace Domain.Entities.Todos;

public class TodoGroupTodo : IAudited
{
    public int TodoGroupTodoId { get; set; }

    public int TodoId { get; set; }
    public int TodoGroupId { get; set; }

    public virtual Todo Todo { get; set; }
    public virtual TodoGroup TodoGroup { get; set; }

    public DateTime CreatedAt { get; set; }
    public int CreatedById { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedById { get; set; }
}
