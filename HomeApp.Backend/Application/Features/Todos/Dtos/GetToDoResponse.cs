using Domain.Entities.Todos;
using Domain.Entities.Todos.Enums;
using SharedKernel.ValueObjects;

namespace Application.Features.Todos.Dtos;

public sealed record GetToDoResponse(
    TodoId TodoId,
    TodoGroupId? TodoGroupId,
    string Name,
    bool Done,
    TodoPriority Priority,
    DateTimeOffset LastModified
)
{
    public static implicit operator GetToDoResponse(Todo item) =>
        new(
            item.TodoId,
            item.TodoGroupTodo?.TodoGroupId,
            item.Name,
            item.Done,
            item.Priority,
            item.LastModified
        );
}
