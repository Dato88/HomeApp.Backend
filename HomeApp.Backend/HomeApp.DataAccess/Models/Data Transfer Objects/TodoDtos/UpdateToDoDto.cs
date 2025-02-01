using HomeApp.DataAccess.enums;

namespace HomeApp.DataAccess.Models.Data_Transfer_Objects.TodoDtos;

public class UpdateToDoDto
{
    public UpdateToDoDto() { }

    public UpdateToDoDto(string name) => Name = name;

    public int Id { get; set; }
    public int? TodoGroupId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Done { get; set; }

    public TodoPriority Priority { get; set; }
    public DateTimeOffset LastModified { get; set; }

    public static implicit operator Todo(UpdateToDoDto item) =>
        new()
        {
            Id = item.Id,
            Name = item.Name,
            Done = item.Done,
            Priority = item.Priority,
            LastModified = DateTimeOffset.Now
        };
}
