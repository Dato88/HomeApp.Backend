using SharedKernel;

namespace Domain.Entities.Todos;

public class TodoGroupTodo : AuditableEntity
{
    public int TodoGroupTodoId { get; set; }

    public int TodoId { get; set; }
    public int TodoGroupId { get; set; }

    public virtual Todo? Todo { get; set; }
    public virtual TodoGroup? TodoGroup { get; set; }
}
