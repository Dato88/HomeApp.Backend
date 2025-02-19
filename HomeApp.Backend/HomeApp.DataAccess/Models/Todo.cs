﻿#nullable disable
using HomeApp.DataAccess.Enums;

namespace HomeApp.DataAccess.Models;

[Table("Todos")]
public class Todo : BaseClass
{
    [Required] [StringLength(150)] public string Name { get; set; }

    [Required] public bool Done { get; set; }

    [Required] public TodoPriority Priority { get; set; }

    public DateTime LastModified { get; set; } = DateTime.UtcNow;

    public virtual TodoGroupTodo TodoGroupTodo { get; set; }
    public virtual ICollection<TodoPerson> TodoPeople { get; set; } = new HashSet<TodoPerson>();
}
