using System.ComponentModel.DataAnnotations;
using HomeApp.DataAccess.enums;

namespace HomeApp.Library.Models.Data_Transfer_Objects.TodoDtos;

public class CreateToDoDto
{
    public CreateToDoDto() { }

    public CreateToDoDto(string name) => Name = name;

    public int? TodoGroupId { get; set; }

    [Required] [StringLength(150)] public string Name { get; set; } = string.Empty;

    [Required] public bool Done { get; set; }

    [Required] public TodoPriority Priority { get; set; }

    public static implicit operator Todo(CreateToDoDto item) =>
        new() { Name = item.Name, Done = item.Done, Priority = item.Priority, LastModified = DateTimeOffset.UtcNow };
}
