using Domain.Entities.Finance.Enums;
using Domain.Entities.People;
using SharedKernel;

namespace Domain.Entities.Finance;

public class Account : AuditableEntity
{
    public int AccountId { get; set; }

    public int PersonId { get; set; }
    public string Name { get; set; } = default!;
    public string? Iban { get; set; }
    public string? Bic { get; set; }
    public AccountType AccountType { get; set; }
    public string CurrencyCode { get; set; } = "EUR";
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    // The account counts as deactivated from this date on; only set while IsActive is false
    public DateOnly? DeactivatedFrom { get; set; }

    public virtual Person? Person { get; set; }
    public virtual ICollection<AccountHousehold> AccountHouseholds { get; set; } = new HashSet<AccountHousehold>();
    public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
}
