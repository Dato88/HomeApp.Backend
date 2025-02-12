using HomeApp.Library.Models.BaseModels;
using HomeApp.Library.Todos.Dtos;

namespace HomeApp.Library.Todos.Queries;

public record GetTodoByIdQuery(int Id) : IRequest<BaseResponse<GetToDoDto>>;
