using System.ComponentModel.DataAnnotations;
using HomeApp.DataAccess.enums;
using HomeApp.DataAccess.Models.Data_Transfer_Objects.TodoDtos;
using HomeApp.Library.Models.BaseModels;
using MediatR;

namespace HomeApp.Library.Todos.Commands;

public class CreateTodoCommand : IRequest<BaseResponse<GetToDoDto>>
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
            LastModified = DateTimeOffset.UtcNow,
            TodoGroupTodo = item.TodoGroupId.HasValue
                ? new TodoGroupTodo { TodoGroupId = item.TodoGroupId.Value }
                : null,
            TodoPeople = new List<TodoPerson> { new() { PersonId = item.PersonId } }
        };
}
