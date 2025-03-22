using SharedKernel;

namespace Domain.Budgets;

public class BudgetColumn : BaseClass
{
    public int Index { get; set; }
    public string Name { get; set; }

    public virtual ICollection<BudgetCell> BudgetCells { get; set; } = new HashSet<BudgetCell>();
}
