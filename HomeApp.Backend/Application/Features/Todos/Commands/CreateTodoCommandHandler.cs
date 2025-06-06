using Application.Abstractions.Authentication;
using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Commands;

internal sealed class CreateTodoCommandHandler(
    ITodoCommands todoCommands,
    IUserContext userContext,
    IAppLogger<CreateTodoCommandHandler> logger) : IRequestHandler<CreateTodoCommand, Result<int>>
{
    private readonly ITodoCommands _todoCommands = todoCommands;

    public async Task<Result<int>> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        request.PersonId = userContext.PersonId;

        var result = await _todoCommands.CreateAsync(request, cancellationToken);

        if (result.IsFailure)
        {
            logger.LogWarning($"Creating todo failed: {result.Error}");

            return Result.Failure<int>(result.Error);
        }

        logger.LogInformation($"Creating todo: {result.Value}");

        return result;
    }
}
