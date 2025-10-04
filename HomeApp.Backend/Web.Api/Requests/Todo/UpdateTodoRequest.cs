using Application.Features.Todos.Commands;
using Domain.Entities.Todos.Enums;

namespace Web.Api.Requests.Todo;

public class UpdateTodoRequest
{
    public int TodoId { get; init; }
    public int? TodoGroupId { get; init; }
    public string Title { get; init; } = string.Empty;
    public bool Done { get; init; }

    public TodoPriority Priority { get; init; }

    public static explicit operator UpdateTodoCommand(UpdateTodoRequest item)
        => new()
        {
            TodoId = item.TodoId,
            Title = item.Title,
            Done = item.Done,
            Priority = item.Priority,
            LastModified = DateTime.Now
        };
}
