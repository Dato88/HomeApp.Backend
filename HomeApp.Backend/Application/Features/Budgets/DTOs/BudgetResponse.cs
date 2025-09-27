using Domain.Entities.Budgets;

namespace Application.Features.Budgets.DTOs;

public sealed class BudgetResponse
{
    public BudgetResponse()
    {
        BudgetCells = new List<BudgetCellDto>();
        BudgetGroups = new List<BudgetGroupDto>();
        BudgetRows = new List<BudgetRowDto>();
    }

    public int BudgetId { get; set; }

    public int Year { get; set; }

    public IEnumerable<BudgetCellDto> BudgetCells { get; set; }
    public IEnumerable<BudgetGroupDto> BudgetGroups { get; set; }
    public IEnumerable<BudgetRowDto> BudgetRows { get; set; }

    public static explicit operator BudgetResponse(Budget budget)
    {
        var groups = budget.BudgetGroups ?? Enumerable.Empty<BudgetGroup>();
        var rows = groups.SelectMany(g => g.BudgetRows ?? Enumerable.Empty<BudgetRow>());
        var cells = rows.SelectMany(r => r.Cells ?? Enumerable.Empty<BudgetCell>());

        return new BudgetResponse
        {
            BudgetId = budget.BudgetId,
            Year = budget.Year,
            BudgetGroups = groups.Select(g => (BudgetGroupDto)g).ToList(),
            BudgetRows = rows.Select(r => (BudgetRowDto)r).ToList(),
            BudgetCells = cells.Select(c => (BudgetCellDto)c).ToList()
        };
    }
}
