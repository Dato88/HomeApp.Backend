using Domain.Entities.Budgets;
using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands;

public sealed record CreateBudgetCellCommand(
    int BudgetRowId,
    int Month,
    decimal Amount) : IRequest<Result<int>>
{
    public static explicit operator BudgetCell(CreateBudgetCellCommand item) =>
        new() { BudgetRowId = item.BudgetRowId, Month = item.Month, Amount = item.Amount };
}
