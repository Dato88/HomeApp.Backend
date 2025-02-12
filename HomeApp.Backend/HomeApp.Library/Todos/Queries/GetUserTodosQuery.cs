using HomeApp.Library.Todos.Dtos;

namespace HomeApp.Library.Todos.Queries;

public class GetUserTodosQuery : IRequest<BaseResponse<IEnumerable<GetToDoDto>>>
{
}
