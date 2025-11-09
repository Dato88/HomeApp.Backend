using Domain.Entities.People;
using SharedKernel;

namespace Domain.Entities.Budgets;

public class Budget : AuditableEntity
{
    public int BudgetId { get; set; }

    public int PersonId { get; set; }
    public int Year { get; set; }

    public virtual Person? Person { get; set; }
    public virtual ICollection<BudgetGroup> BudgetGroups { get; set; } = new HashSet<BudgetGroup>();
}
