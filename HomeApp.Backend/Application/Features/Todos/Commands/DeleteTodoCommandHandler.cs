using Application.Abstractions.Logging;
using Domain.Entities.Todos;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Commands;

public class DeleteTodoCommandHandler(
    ITodoCommands todoCommands,
    IAppLogger<DeleteTodoCommandHandler> logger) : IRequestHandler<DeleteTodoCommand, Result<bool>>
{
    private readonly ITodoCommands _todoCommands = todoCommands;

    public async Task<Result<bool>> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var isDeleted = await _todoCommands.DeleteAsync(request.Id, cancellationToken);

            if (!isDeleted)
            {
                logger.LogWarning(TodoErrors.DeleteFailed(request.Id).Description);

                return Result.Failure<bool>(TodoErrors.DeleteFailed(request.Id));
            }

            return isDeleted;
        }
        catch (Exception ex)
        {
            logger.LogError($"Delete todo failed: {ex}");

            return Result.Failure<bool>(TodoErrors.DeleteFailedWithMessage(ex.Message));
        }
    }
}
