using Domain.Entities.Households;
using SharedKernel;

namespace Domain.Entities.Budgets;

public class Budget : AuditableEntity
{
    public int BudgetId { get; set; }

    public int HouseholdId { get; set; }
    public int Year { get; set; }

    public virtual Household? Household { get; set; }
    public virtual ICollection<BudgetGroup> BudgetGroups { get; set; } = new HashSet<BudgetGroup>();
}
