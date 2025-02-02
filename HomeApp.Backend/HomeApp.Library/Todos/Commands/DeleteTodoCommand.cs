using HomeApp.Library.Models.BaseModels;
using MediatR;

namespace HomeApp.Library.Todos.Commands;

public class DeleteTodoCommand(int id) : IRequest<BaseResponse<bool>>
{
    public int Id { get; set; } = id;
}
