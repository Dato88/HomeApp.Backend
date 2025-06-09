using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Commands;

internal sealed class DeleteTodoCommandHandler(
    ITodoCommands todoCommands,
    IAppLogger<DeleteTodoCommandHandler> logger) : IRequestHandler<DeleteTodoCommand, Result>
{
    private readonly ITodoCommands _todoCommands = todoCommands;

    public async Task<Result> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
    {
        var result = await _todoCommands.DeleteAsync(request.Id, cancellationToken);

        if (result.IsFailure)
        {
            foreach (var error in result.Errors)
                logger.LogWarning($"Deleting todo failed: {error.Description} ({error.Code})");

            return Result.Failure(result.Errors.ToArray());
        }

        logger.LogInformation($"Todo with ID {request.Id} deleted successfully.");

        return Result.Success();
    }
}
