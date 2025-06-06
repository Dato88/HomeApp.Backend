using System.ComponentModel.DataAnnotations;
using Application.Features.Todos.Commands;
using Domain.Entities.Todos.Enums;

namespace Web.Api.Requests.Todo;

public sealed record CreateTodoRequest
{
    public int? TodoGroupId { get; set; }
    public int PersonId { get; set; }

    [Required] [StringLength(150)] public string Name { get; set; } = string.Empty;

    [Required] public bool Done { get; set; }

    [Required] public TodoPriority Priority { get; set; }

    public static explicit operator CreateTodoCommand(CreateTodoRequest request)
        => new()
        {
            TodoGroupId = request.TodoGroupId,
            PersonId = request.PersonId,
            Name = request.Name,
            Done = request.Done,
            Priority = request.Priority
        };
}
