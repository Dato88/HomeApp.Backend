using Domain.Entities.Todos;
using Domain.Entities.Todos.Enums;
using MediatR;
using SharedKernel;
using SharedKernel.ValueObjects;

namespace Application.Features.Todos.Commands;

public sealed class UpdateTodoCommand : IRequest<Result>
{
    public TodoId TodoId { get; set; }
    public int? TodoGroupId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Done { get; set; }

    public TodoPriority Priority { get; set; }
    public DateTimeOffset LastModified { get; set; }

    public static implicit operator Todo(UpdateTodoCommand item) =>
        new()
        {
            TodoId = item.TodoId,
            Name = item.Name,
            Done = item.Done,
            Priority = item.Priority,
            LastModified = DateTime.Now
        };
}
