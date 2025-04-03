using Application.Abstractions.Logging;
using Application.Features.Todos.Dtos;
using Domain.Entities.Todos;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Queries;

public class GetTodoByIdQueryHandler(
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
            var todo = await _todoQueries.FindByIdAsync(request.Id, cancellationToken);

            if (todo is null) return Result.Failure<GetToDoResponse>(TodoErrors.NotFoundById(request.Id));

            return (GetToDoResponse)todo;
        }
        catch (Exception ex)
        {
            logger.LogError($"Get todo failed: {ex}");

            return Result.Failure<GetToDoResponse>(TodoErrors.NotFoundWithMessage(ex.Message));
        }
    }
}
