using HomeApp.DataAccess.enums;
using HomeApp.Library.Models.BaseModels;
using HomeApp.Library.Models.TodoDtos;
using MediatR;

namespace HomeApp.Library.Todos.Commands;

public class UpdateTodoCommand : IRequest<BaseResponse<GetToDoDto>>
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
            LastModified = DateTimeOffset.Now
        };
}
