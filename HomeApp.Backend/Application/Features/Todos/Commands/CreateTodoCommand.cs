using System.ComponentModel.DataAnnotations;
using Domain.Entities.Todos;
using Domain.Entities.Todos.Enums;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Commands;

public sealed record CreateTodoCommand : IRequest<Result<int>>
{
    public int? TodoGroupId { get; set; }
    public int PersonId { get; set; }

    [Required] [StringLength(150)] public string Title { get; set; } = default!;

    [Required] public bool Done { get; set; }

    [Required] public TodoPriority Priority { get; set; }

    public static explicit operator Todo(CreateTodoCommand item) =>
        new()
        {
            Title = item.Title,
            Done = item.Done,
            Priority = item.Priority,
            TodoGroupTodo = item.TodoGroupId.HasValue
                ? new TodoGroupTodo { TodoGroupId = item.TodoGroupId.Value }
                : null,
            TodoPeople = new List<TodoPerson> { new() { CreatedById = item.PersonId, PersonId = item.PersonId } },
            CreatedById = item.PersonId
        };
}
