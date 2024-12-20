#nullable disable
namespace HomeApp.DataAccess.Models;

[Table("TodoGroups")]
public class TodoGroup : BaseClass
{
    public TodoGroup() => Todos = new HashSet<TodoGroupTodo>();

    [Required] [StringLength(150)] public string Name { get; set; }

    public virtual ICollection<TodoGroupTodo> Todos { get; set; }
}
