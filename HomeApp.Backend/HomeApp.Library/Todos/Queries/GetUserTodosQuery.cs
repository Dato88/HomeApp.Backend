using HomeApp.Library.Models.BaseModels;
using HomeApp.Library.Models.TodoDtos;
using MediatR;

namespace HomeApp.Library.Todos.Queries;

public class GetUserTodosQuery : IRequest<BaseResponse<IEnumerable<GetToDoDto>>>
{
}
