using Domain.Entities.Todos;
using Domain.Entities.Todos.Enums;

namespace Application.Features.Todos.Dtos;

public sealed record GetToDoResponse(
    int TodoId,
    int? TodoGroupId,
    string Name,
    bool Done,
    TodoPriority Priority,
    DateTime? UpdatedAt
)
{
    public static implicit operator GetToDoResponse(Todo item) =>
        new(
            item.TodoId,
            item.TodoGroupTodo?.TodoGroupId,
            item.Name,
            item.Done,
            item.Priority,
            item.UpdatedAt
        );
}
