using Application.Abstractions.Logging;
using Domain.Entities.Todos;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Commands;

public class UpdateTodoCommandHandler(
    ITodoCommands todoCommands,
    IAppLogger<UpdateTodoCommandHandler> logger) : IRequestHandler<UpdateTodoCommand, Result<bool>>
{
    private readonly ITodoCommands _todoCommands = todoCommands;

    public async Task<Result<bool>> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var isUpdated = await _todoCommands.UpdateAsync(request, cancellationToken);

            if (!isUpdated) return Result.Failure<bool>(TodoErrors.UpdateFailed(request.Id));

            return isUpdated;
        }
        catch (Exception ex)
        {
            logger.LogError($"Update todo failed: {ex}");

            return Result.Failure<bool>(TodoErrors.UpdateFailedWithMessage(ex.Message));
        }
    }
}
