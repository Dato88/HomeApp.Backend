using Domain.Entities.Finance.Enums;
using Domain.Entities.Households;
using SharedKernel;

namespace Domain.Entities.Finance;

public class Category : AuditableEntity
{
    public int CategoryId { get; set; }

    public int HouseholdId { get; set; }
    public string Name { get; set; } = default!;
    public CategoryType CategoryType { get; set; }

    public virtual Household? Household { get; set; }
    public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
}
