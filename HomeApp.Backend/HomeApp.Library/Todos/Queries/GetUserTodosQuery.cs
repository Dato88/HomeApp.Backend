using HomeApp.DataAccess.Models.Data_Transfer_Objects.TodoDtos;
using HomeApp.Library.Models.BaseModels;
using MediatR;

namespace HomeApp.Library.Todos.Queries;

public class GetUserTodosQuery : IRequest<BaseResponse<IEnumerable<GetToDoDto>>>
{
}
