using Domain.Entities.Todos;
using Domain.Entities.Todos.Enums;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Commands;

public class UpdateTodoCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
    public int? TodoGroupId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Done { get; set; }

    public TodoPriority Priority { get; set; }
    public DateTimeOffset LastModified { get; set; }

    public static implicit operator Todo(UpdateTodoCommand item) =>
        new()
        {
            Id = item.Id,
            Name = item.Name,
            Done = item.Done,
            Priority = item.Priority,
            LastModified = DateTime.Now
        };
}
