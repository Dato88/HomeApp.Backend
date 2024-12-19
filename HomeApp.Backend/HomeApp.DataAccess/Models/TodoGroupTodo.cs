#nullable disable
namespace HomeApp.DataAccess.Models;

[Table("TodoGroupTodos")]
public class TodoGroupTodo : BaseClass
{
    [Required] public int TodoId { get; set; }

    [Required] public int TodoGroupId { get; set; }

    public virtual Todo Todo { get; set; }
    public virtual TodoGroup TodoGroup { get; set; }
}
