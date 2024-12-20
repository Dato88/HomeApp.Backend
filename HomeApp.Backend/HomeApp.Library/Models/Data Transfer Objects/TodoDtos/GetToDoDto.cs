using HomeApp.DataAccess.enums;

namespace HomeApp.Library.Models.Data_Transfer_Objects.TodoDtos;

public class GetToDoDto
{
    public GetToDoDto() { }

    public GetToDoDto(string name) => Name = name;

    public int Id { get; set; }
    public int? TodoGroupId { get; set; }
    public string Name { get; set; }
    public bool Done { get; set; }

    public TodoPriority Priority { get; set; }
    public DateTime? ExecutionDate { get; set; }

    public static implicit operator GetToDoDto(Todo item) =>
        new()
        {
            Id = item.Id,
            TodoGroupId = item.TodoGroupTodo?.TodoGroupId,
            Name = item.Name,
            Done = item.Done,
            Priority = item.Priority,
            ExecutionDate = item.ExecutionDate
        };
}
