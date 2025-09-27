using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands;

public sealed class DeleteBudgetCommandHandler(
    IBudgetCommands budgetCommands,
    IAppLogger<DeleteBudgetCommandHandler> logger)
    : IRequestHandler<DeleteBudgetCommand, Result<int>>
{
    private readonly IBudgetCommands _budgetCommands = budgetCommands;
    private readonly IAppLogger<DeleteBudgetCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(DeleteBudgetCommand request, CancellationToken cancellationToken)
    {
        var result = await _budgetCommands.DeleteBudgetAsync(request.BudgetId, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Deleting budget failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Deleting budget: {result.Value}");

        return result;
    }
}
