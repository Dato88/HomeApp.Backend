using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands;

public sealed class DeleteBudgetGroupCommandHandler(
    IBudgetCommands budgetCommands,
    IAppLogger<DeleteBudgetGroupCommandHandler> logger)
    : IRequestHandler<DeleteBudgetGroupCommand, Result<int>>
{
    private readonly IBudgetCommands _budgetCommands = budgetCommands;
    private readonly IAppLogger<DeleteBudgetGroupCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(DeleteBudgetGroupCommand request, CancellationToken cancellationToken)
    {
        var result = await _budgetCommands.DeleteBudgetGroupAsync(request.BudgetGroupId, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Deleting budgetGroup failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Deleting budgetGroup: {result.Value}");

        return result;
    }
}
