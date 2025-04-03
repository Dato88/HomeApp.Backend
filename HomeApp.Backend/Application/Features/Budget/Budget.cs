using Domain.Entities.Budgets;

namespace Application.Features.Budget;

public class Budget
{
    public Budget()
    {
        BudgetCells = new List<BudgetCell>();
        BudgetColumns = new List<BudgetColumn>();
        BudgetGroups = new List<BudgetGroup>();
        BudgetRows = new List<BudgetRow>();
    }

    public IEnumerable<BudgetCell> BudgetCells { get; set; }
    public IEnumerable<BudgetColumn> BudgetColumns { get; set; }
    public IEnumerable<BudgetGroup> BudgetGroups { get; set; }
    public IEnumerable<BudgetRow> BudgetRows { get; set; }
}
