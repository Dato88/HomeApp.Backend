using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands;

public sealed class DeleteBudgetCellCommandHandler(
    IBudgetCommands budgetCommands,
    IAppLogger<DeleteBudgetCellCommandHandler> logger)
    : IRequestHandler<DeleteBudgetCellCommand, Result<int>>
{
    private readonly IBudgetCommands _budgetCommands = budgetCommands;
    private readonly IAppLogger<DeleteBudgetCellCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(DeleteBudgetCellCommand request, CancellationToken cancellationToken)
    {
        var result = await _budgetCommands.DeleteBudgetCellAsync(request.BudgetCellId, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Deleting budgetCell failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Deleting budgetCell: {result.Value}");

        return result;
    }
}
