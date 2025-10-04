using Domain.Entities.Todos;
using Domain.Entities.Todos.Enums;

namespace Application.Features.Todos.Dtos;

public sealed record GetToDoResponse(
    int TodoId,
    int? TodoGroupId,
    string Title,
    bool Done,
    TodoPriority Priority,
    DateTime? LastModified
)
{
    public static explicit operator GetToDoResponse(Todo item) =>
        new(
            item.TodoId,
            item.TodoGroupTodo?.TodoGroupId,
            item.Title,
            item.Done,
            item.Priority,
            item.UpdatedAt
        );
}
