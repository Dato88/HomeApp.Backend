#nullable disable
using HomeApp.DataAccess.enums;

namespace HomeApp.DataAccess.Models;

[Table("Todos")]
public class Todo : BaseClass
{
    public Todo()
    {
        TodoGroups = new HashSet<TodoGroupTodo>();
        People = new HashSet<TodoPerson>();
    }

    [Required] [StringLength(150)] public string Name { get; set; }

    [Required] public bool Done { get; set; }

    [Required] public TodoPriority Priority { get; set; }

    public DateTime ExecutionDate { get; set; }

    public virtual ICollection<TodoGroupTodo> TodoGroups { get; set; }
    public virtual ICollection<TodoPerson> People { get; set; }
}
