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
            logger.LogWarning($"Deleting todo failed: {result.Error}");
            return Result.Failure(result.Error);
        }

        logger.LogInformation($"Todo with ID {request.Id} deleted successfully.");

        return Result.Success();
    }
}
