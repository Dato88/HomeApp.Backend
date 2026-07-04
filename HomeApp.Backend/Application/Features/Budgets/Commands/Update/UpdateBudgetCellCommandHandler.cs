using Application.Abstractions.BudgetModule;
using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands.Update;

public sealed class UpdateBudgetCellCommandHandler(
    IBudgetCommands budgetCommands,
    IAppLogger<UpdateBudgetCellCommandHandler> logger)
    : IRequestHandler<UpdateBudgetCellCommand, Result<int>>
{
    private readonly IBudgetCommands _budgetCommands = budgetCommands;
    private readonly IAppLogger<UpdateBudgetCellCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(UpdateBudgetCellCommand request, CancellationToken cancellationToken)
    {
        var result = await _budgetCommands.UpdateBudgetCellAsync(request.BudgetCellId, request.Amount,
            cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Updating budgetcell failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Updating budgetcell: {result.Value}");

        return result;
    }
}
