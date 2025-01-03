﻿#nullable disable
namespace HomeApp.DataAccess.Models;

[Table("TodoGroups")]
public class TodoGroup : BaseClass
{
    [Required] [StringLength(150)] public string Name { get; set; }

    public virtual ICollection<TodoGroupTodo> TodoGroupTodos { get; set; } = new HashSet<TodoGroupTodo>();
}
