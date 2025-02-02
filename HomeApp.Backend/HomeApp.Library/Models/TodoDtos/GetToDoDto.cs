using HomeApp.DataAccess.enums;

namespace HomeApp.Library.Models.TodoDtos;

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
