using Application.Abstractions.Authentication;
using Application.Abstractions.Logging;
using Domain.Entities.Todos;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Commands;

internal sealed class UpdateTodoCommandHandler(
    ITodoCommands todoCommands,
    IExecutionContextAccessor executionContext,
    IAppLogger<UpdateTodoCommandHandler> logger) : IRequestHandler<UpdateTodoCommand, Result>
{
    private readonly ITodoCommands _todoCommands = todoCommands;

    public async Task<Result> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        request.PersonId = executionContext.PersonId;

        var result = await _todoCommands.UpdateAsync((Todo)request, cancellationToken);

        if (result.IsFailure)
        {
            logger.LogWarning($"Updating todo failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure(result.Error);
        }

        logger.LogInformation($"Todo with ID {request.TodoId} updated successfully.");

        return Result.Success();
    }
}
