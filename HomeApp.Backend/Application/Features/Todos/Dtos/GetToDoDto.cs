using Domain.Entities.Todos;
using Domain.Entities.Todos.Enums;

namespace Application.Features.Todos.Dtos;

public record GetToDoDto(
    int Id,
    int? TodoGroupId,
    string Name,
    bool Done,
    TodoPriority Priority,
    DateTimeOffset LastModified
)
{
    public static implicit operator GetToDoDto(Todo item) =>
        new(
            item.Id,
            item.TodoGroupTodo?.TodoGroupId,
            item.Name,
            item.Done,
            item.Priority,
            item.LastModified
        );
}
