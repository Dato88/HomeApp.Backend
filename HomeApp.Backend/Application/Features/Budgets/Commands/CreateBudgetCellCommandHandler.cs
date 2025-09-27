using Application.Abstractions.Logging;
using Domain.Entities.Budgets;
using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands;

public class CreateBudgetCellCommandHandler(
    IBudgetCommands budgetCommands,
    IAppLogger<CreateBudgetCellCommandHandler> logger)
    : IRequestHandler<CreateBudgetCellCommand, Result<int>>
{
    private readonly IBudgetCommands _budgetCommands = budgetCommands;
    private readonly IAppLogger<CreateBudgetCellCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(CreateBudgetCellCommand request, CancellationToken cancellationToken)
    {
        var result = await _budgetCommands.CreateBudgetCellAsync((BudgetCell)request, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Creating budgetCell failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Creating budgetRow: {result.Value}");

        return result;
    }
}
