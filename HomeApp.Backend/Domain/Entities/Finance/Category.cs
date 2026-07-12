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

    // Optional grouping (e.g. "Wohnen", "Sparen") for the E+A report
    public int? CategoryGroupId { get; set; }

    public virtual Household? Household { get; set; }
    public virtual CategoryGroup? CategoryGroup { get; set; }
    public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
}
