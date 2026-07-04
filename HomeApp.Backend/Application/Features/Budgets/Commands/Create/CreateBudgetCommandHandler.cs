using Application.Abstractions.BudgetModule;
using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands.Create;

public sealed class CreateBudgetCommandHandler(
    IBudgetCommands budgetCommands,
    IAppLogger<CreateBudgetCommandHandler> logger)
    : IRequestHandler<CreateBudgetCommand, Result<int>>
{
    private readonly IBudgetCommands _budgetCommands = budgetCommands;
    private readonly IAppLogger<CreateBudgetCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
    {
        var result = await _budgetCommands.CreateBudgetAsync(request.HouseholdId, request.Year, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Creating budget failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Creating budget: {result.Value}");

        return result;
    }
}
