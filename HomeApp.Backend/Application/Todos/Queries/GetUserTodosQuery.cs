using Application.Todos.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Todos.Queries;

public class GetUserTodosQuery : IRequest<BaseResponse<IEnumerable<GetToDoDto>>>
{
}
