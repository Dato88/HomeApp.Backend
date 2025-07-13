using Domain.Entities.People;
using SharedKernel;

namespace Domain.Entities.Budgets;

public class BudgetRow : BaseClass
{
    public int PersonId { get; set; }
    public int Index { get; set; }
    public int Year { get; set; }
    public string Name { get; set; }

    public virtual Person Person { get; set; }
    public virtual ICollection<BudgetCell> BudgetCells { get; set; } = new HashSet<BudgetCell>();
}
