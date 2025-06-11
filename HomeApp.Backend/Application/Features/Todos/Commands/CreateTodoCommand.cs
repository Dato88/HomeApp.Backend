using System.ComponentModel.DataAnnotations;
using Domain.Entities.Todos;
using Domain.Entities.Todos.Enums;
using MediatR;
using SharedKernel;
using SharedKernel.ValueObjects;

namespace Application.Features.Todos.Commands;

public sealed record CreateTodoCommand : IRequest<Result<int>>
{
    public TodoGroupId? TodoGroupId { get; set; }
    public PersonId PersonId { get; set; }

    [Required] [StringLength(150)] public string Name { get; set; } = string.Empty;

    [Required] public bool Done { get; set; }

    [Required] public TodoPriority Priority { get; set; }

    public static implicit operator Todo(CreateTodoCommand item) =>
        new()
        {
            Name = item.Name,
            Done = item.Done,
            Priority = item.Priority,
            LastModified = DateTime.UtcNow,
            TodoGroupTodo = item.TodoGroupId.HasValue
                ? new TodoGroupTodo { TodoGroupId = item.TodoGroupId.Value }
                : null,
            TodoPeople = new List<TodoPerson> { new() { PersonId = item.PersonId } }
        };
}
