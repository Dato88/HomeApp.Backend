using Domain.Entities.People;
using Domain.ValueObjects;
using SharedKernel;

namespace Domain.Entities.Budgets;

public class BudgetGroup : BaseClass
{
    public PersonId PersonId { get; set; }
    public int Index { get; set; }
    public string Name { get; set; }

    public virtual Person Person { get; set; }
    public virtual ICollection<BudgetCell> BudgetCells { get; set; } = new HashSet<BudgetCell>();
}
