using SharedKernel;

namespace Domain.Entities.Todos;

public class TodoGroup : BaseClass
{
    public string Name { get; set; }

    public virtual ICollection<TodoGroupTodo> TodoGroupTodos { get; set; } = new HashSet<TodoGroupTodo>();
}
