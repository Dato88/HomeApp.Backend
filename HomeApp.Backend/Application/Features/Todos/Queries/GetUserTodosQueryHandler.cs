using Application.Abstractions.Authentication;
using Application.Abstractions.Logging;
using Application.Features.People.Queries;
using Application.Features.Todos.Dtos;
using Domain.Entities.Todos;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Queries;

internal sealed class GetUserTodosQueryHandler(
    ITodoQueries todoQueries,
    IUserContext userContext,
    IPersonQueries personQueries,
    IAppLogger<GetUserTodosQueryHandler> logger)
    : IRequestHandler<GetUserTodosQuery, Result<IEnumerable<GetToDoResponse>>>
{
    public async Task<Result<IEnumerable<GetToDoResponse>>> Handle(GetUserTodosQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var todosResult = await todoQueries.GetAllAsync(userContext.PersonId, cancellationToken);

            var result = todosResult.Value.Select(s => (GetToDoResponse)s);

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            logger.LogError($"Get todos failed: {ex}");

            return Result.Failure<IEnumerable<GetToDoResponse>>(TodoErrors.NotFoundAll);
        }
    }
}
