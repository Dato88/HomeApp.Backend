using Domain.Entities.Budgets;

namespace Application.Features.Budgets.DTOs;

public class BudgetCellDto
{
    public int BudgetCellId { get; set; }

    public int BudgetRowId { get; set; }
    public int Month { get; set; }
    public decimal Amount { get; set; }

    public static explicit operator BudgetCellDto(BudgetCell entity) =>
        new()
        {
            BudgetCellId = entity.BudgetCellId,
            BudgetRowId = entity.BudgetRowId,
            Month = entity.Month,
            Amount = entity.Amount,
        };
}
