using Application.Features.Todos.Commands;
using Domain.Entities.Todos.Enums;

namespace Web.Api.Requests.Todo;

public class UpdateTodoRequest
{
    public int Id { get; set; }
    public int? TodoGroupId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Done { get; set; }

    public TodoPriority Priority { get; set; }
    public DateTimeOffset LastModified { get; set; }

    public static explicit operator UpdateTodoCommand(UpdateTodoRequest item)
        => new()
        {
            Id = item.Id,
            Name = item.Name,
            Done = item.Done,
            Priority = item.Priority,
            LastModified = DateTime.Now
        };
}
