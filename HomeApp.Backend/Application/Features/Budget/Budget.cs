using Domain.Entities.Budgets;

namespace Application.Features.Budget;

internal sealed class Budget
{
    public Budget()
    {
        BudgetCells = new List<BudgetCell>();
        Budgets = new List<Budget>();
        BudgetGroups = new List<BudgetGroup>();
        BudgetRows = new List<BudgetRow>();
    }

    public IEnumerable<BudgetCell> BudgetCells { get; set; }
    public IEnumerable<Budget> Budgets { get; set; }
    public IEnumerable<BudgetGroup> BudgetGroups { get; set; }
    public IEnumerable<BudgetRow> BudgetRows { get; set; }
}
