using SharedKernel.ValueObjects;

namespace Domain.Entities.Todos;

public class TodoGroupTodo
{
    public TodoGroupTodoId TodoGroupTodoId { get; set; }
    public DateTime CreatedAt { get; set; }
    public TodoId TodoId { get; set; }
    public TodoGroupId TodoGroupId { get; set; }

    public virtual Todo Todo { get; set; }
    public virtual TodoGroup TodoGroup { get; set; }
}
