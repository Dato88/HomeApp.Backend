using Application.Abstractions.Logging;
using Domain.Entities.Budgets;
using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands.Create;

public sealed class CreateBudgetRowCommandHandler(
    IBudgetCommands budgetCommands,
    IAppLogger<CreateBudgetRowCommandHandler> logger)
    : IRequestHandler<CreateBudgetRowCommand, Result<int>>
{
    private readonly IBudgetCommands _budgetCommands = budgetCommands;
    private readonly IAppLogger<CreateBudgetRowCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(CreateBudgetRowCommand request, CancellationToken cancellationToken)
    {
        var result = await _budgetCommands.CreateBudgetRowAsync((BudgetRow)request, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Creating budgetRow failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Creating budgetRow: {result.Value}");

        return result;
    }
}
