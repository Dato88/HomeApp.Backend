#nullable disable
namespace HomeApp.DataAccess.Models;

[Table("TodoGroups")]
public partial class TodoGroup : BaseClass
{
    public TodoGroup() => TodoGroupMappings = new HashSet<TodoGroupMapping>();

    [Required]
    [StringLength(150)]
    public string TodoGroupName { get; set; }

    public virtual ICollection<TodoGroupMapping> TodoGroupMappings { get; set; }
}
