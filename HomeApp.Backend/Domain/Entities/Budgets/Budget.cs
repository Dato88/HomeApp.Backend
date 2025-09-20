using Domain.Entities.People;
using SharedKernel;

namespace Domain.Entities.Budgets;

public class Budget : IAudited
{
    public int BudgetId { get; set; }

    public int PersonId { get; set; }
    public int Year { get; set; }

    public virtual Person Person { get; set; }
    public virtual ICollection<BudgetGroup> BudgetGroups { get; set; } = new HashSet<BudgetGroup>();

    public DateTime CreatedAt { get; set; }
    public int CreatedById { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedById { get; set; }
}
