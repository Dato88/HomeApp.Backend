using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Commands;

internal sealed class UpdateTodoCommandHandler(
    ITodoCommands todoCommands,
    IAppLogger<UpdateTodoCommandHandler> logger) : IRequestHandler<UpdateTodoCommand, Result>
{
    private readonly ITodoCommands _todoCommands = todoCommands;

    public async Task<Result> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        var result = await _todoCommands.UpdateAsync(request, cancellationToken);

        if (result.IsFailure)
        {
            foreach (var error in result.Errors)
                logger.LogWarning($"Updating todo failed: {error.Description} ({error.Code})");

            return Result.Failure(result.Errors.ToArray());
        }

        logger.LogInformation($"Todo with ID {request.Id} updated successfully.");

        return Result.Success();
    }
}
