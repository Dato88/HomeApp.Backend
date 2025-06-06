using Application.Abstractions.Logging;
using Application.Features.Todos.Dtos;
using Domain.Entities.Todos;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Queries;

internal sealed class GetTodoByIdQueryHandler(
    ITodoQueries todoQueries,
    IAppLogger<GetTodoByIdQueryHandler> logger)
    : IRequestHandler<GetTodoByIdQuery, Result<GetToDoResponse>>
{
    private readonly ITodoQueries _todoQueries = todoQueries;

    public async Task<Result<GetToDoResponse>> Handle(GetTodoByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var todoResult = await _todoQueries.FindByIdAsync(request.Id, cancellationToken);

            if (todoResult.IsFailure)
            {
                logger.LogWarning(TodoErrors.NotFoundById(request.Id).Description);

                return Result.Failure<GetToDoResponse>(TodoErrors.NotFoundById(request.Id));
            }

            return (GetToDoResponse)todoResult.Value;
        }
        catch (Exception ex)
        {
            logger.LogError($"Get todo failed: {ex}");

            return Result.Failure<GetToDoResponse>(TodoErrors.NotFoundWithMessage(ex.Message));
        }
    }
}
