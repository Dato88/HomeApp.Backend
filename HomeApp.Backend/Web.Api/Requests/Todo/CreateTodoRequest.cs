using Application.Features.Todos.Commands;
using Domain.Entities.Todos.Enums;

namespace Web.Api.Requests.Todo;

public sealed record CreateTodoRequest
{
    public int? TodoGroupId { get; init; }
    public string Name { get; init; } = string.Empty;
    public bool Done { get; init; }
    public TodoPriority Priority { get; init; }

    public static explicit operator CreateTodoCommand(CreateTodoRequest request)
        => new()
        {
            TodoGroupId = request.TodoGroupId, Name = request.Name, Done = request.Done, Priority = request.Priority
        };
}
