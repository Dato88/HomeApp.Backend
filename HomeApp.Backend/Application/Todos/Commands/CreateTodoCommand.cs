using System.ComponentModel.DataAnnotations;
using Domain.Todos;
using Domain.Todos.Enums;
using MediatR;
using SharedKernel;

namespace Application.Todos.Commands;

public class CreateTodoCommand : IRequest<BaseResponse<int>>
{
    public int? TodoGroupId { get; set; }
    public int PersonId { get; set; }

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
