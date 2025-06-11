using Application.Features.Todos.Commands;
using Domain.Entities.Todos.Enums;
using SharedKernel.ValueObjects;

namespace Web.Api.Requests.Todo;

public class UpdateTodoRequest
{
    public TodoId TodoId { get; set; }
    public int? TodoGroupId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Done { get; set; }

    public TodoPriority Priority { get; set; }

    public static explicit operator UpdateTodoCommand(UpdateTodoRequest item)
        => new()
        {
            TodoId = item.TodoId,
            Name = item.Name,
            Done = item.Done,
            Priority = item.Priority,
            LastModified = DateTime.Now
        };
}
