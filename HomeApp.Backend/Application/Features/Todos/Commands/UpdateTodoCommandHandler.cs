using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Commands;

public class UpdateTodoCommandHandler(
    ITodoCommands todoCommands,
    IAppLogger<UpdateTodoCommandHandler> logger) : IRequestHandler<UpdateTodoCommand, Result>
{
    private readonly ITodoCommands _todoCommands = todoCommands;

    public async Task<Result> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        var result = await _todoCommands.UpdateAsync(request, cancellationToken);

        if (result.IsFailure)
        {
            logger.LogWarning($"Updating todo failed: {result.Error}");
            return Result.Failure(result.Error);
        }

        logger.LogInformation($"Todo with ID {request.Id} updated successfully.");

        return Result.Success();
    }
}
