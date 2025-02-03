using HomeApp.Library.Models.BaseModels;
using MediatR;

namespace HomeApp.Library.Todos.Commands;

public record DeleteTodoCommand(int Id) : IRequest<BaseResponse<bool>>;
