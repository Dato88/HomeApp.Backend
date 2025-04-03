using Application.Abstractions.Authentication;
using Application.Abstractions.Logging;
using Domain.Entities.Todos;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Commands;

public class CreateTodoCommandHandler(
    ITodoCommands todoCommands,
    IUserContext userContext,
    IAppLogger<CreateTodoCommandHandler> logger) : IRequestHandler<CreateTodoCommand, Result<int>>
{
    private readonly ITodoCommands _todoCommands = todoCommands;

    public async Task<Result<int>> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            request.PersonId = userContext.PersonId;

            var response = await _todoCommands.CreateAsync(request, cancellationToken);

            if (response == 0)
            {
                logger.LogWarning(TodoErrors.CreateFailed.Description);

                return Result.Failure<int>(TodoErrors.CreateFailed);
            }

            logger.LogInformation($"Creating todo: {response}");

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError($"Creating todo failed: {ex}");

            return Result.Failure<int>(TodoErrors.CreateFailedWithMessage(ex.Message));
        }
    }
}
