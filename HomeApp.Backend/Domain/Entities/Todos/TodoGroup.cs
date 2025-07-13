namespace Domain.Entities.Todos;

public class TodoGroup
{
    public int TodoGroupId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Name { get; set; }

    public virtual ICollection<TodoGroupTodo> TodoGroupTodos { get; set; } = new HashSet<TodoGroupTodo>();
}
