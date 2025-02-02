using HomeApp.Library.Models.BaseModels;
using HomeApp.Library.Models.TodoDtos;
using MediatR;

namespace HomeApp.Library.Todos.Queries;

public record GetTodoByIdQuery(int Id) : IRequest<BaseResponse<GetToDoDto>>;
