using Application.Features.Todos.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Queries;

public sealed class GetUserTodosQuery : IRequest<Result<IEnumerable<GetToDoResponse>>>
{
}
