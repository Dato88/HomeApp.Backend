using Application.Abstractions.Authentication;
using Application.Abstractions.Logging;
using Domain.Entities.Todos;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Commands;

internal sealed class CreateTodoCommandHandler(
    ITodoCommands todoCommands,
    IExecutionContextAccessor executionContext,
    IAppLogger<CreateTodoCommandHandler> logger) : IRequestHandler<CreateTodoCommand, Result<int>>
{
    private readonly ITodoCommands _todoCommands = todoCommands;

    public async Task<Result<int>> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        request.PersonId = executionContext.PersonId;

        var result = await _todoCommands.CreateAsync((Todo)request, cancellationToken);

        if (result.IsFailure)
        {
            logger.LogWarning($"Creating todo failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        logger.LogInformation($"Creating todo: {result.Value}");

        return result;
    }
}
