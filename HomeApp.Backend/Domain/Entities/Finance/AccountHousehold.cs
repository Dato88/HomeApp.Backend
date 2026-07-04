using Domain.Entities.Households;
using SharedKernel;

namespace Domain.Entities.Finance;

public class AccountHousehold : AuditableEntity
{
    public int AccountHouseholdId { get; set; }

    public int AccountId { get; set; }
    public int HouseholdId { get; set; }

    public virtual Account? Account { get; set; }
    public virtual Household? Household { get; set; }
}
