using Application.Abstractions.BudgetModule;
using Application.Abstractions.Logging;
using Domain.Entities.Budgets;
using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands.Update;

public sealed class UpdateBudgetCommandHandler(
    IBudgetCommands budgetCommands,
    IAppLogger<UpdateBudgetCommandHandler> logger)
    : IRequestHandler<UpdateBudgetCommand, Result<int>>
{
    private readonly IBudgetCommands _budgetCommands = budgetCommands;
    private readonly IAppLogger<UpdateBudgetCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(UpdateBudgetCommand request, CancellationToken cancellationToken)
    {
        var result = await _budgetCommands.UpdateBudgetAsync(request.BudgetId, request.Year, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Updating budget failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Updating budget: {result.Value}");

        return result;
    }
}
