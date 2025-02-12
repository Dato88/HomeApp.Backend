using HomeApp.Library.Models.BaseModels;

namespace HomeApp.Library.Todos.Commands;

public record DeleteTodoCommand(int Id) : IRequest<BaseResponse<bool>>;
