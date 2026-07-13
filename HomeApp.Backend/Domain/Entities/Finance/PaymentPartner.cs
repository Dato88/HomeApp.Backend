using Domain.Entities.People;
using SharedKernel;

namespace Domain.Entities.Finance;

public class PaymentPartner : AuditableEntity
{
    public int PaymentPartnerId { get; set; }

    // Scoped by the account owner, not by household: an account can be shared
    // into 0..n households, so only the owner is a deterministic scope.
    public int PersonId { get; set; }

    public string DisplayName { get; set; } = default!;

    // Matching key derived from the raw bank name; stays stable when the user
    // renames the partner so future imports keep matching.
    public string NormalizedName { get; set; } = default!;

    // Normalized IBAN; when present it is the partner's identity.
    public string? Iban { get; set; }

    // Set when the IBAN belongs to one of the owner's own accounts (self-transfer)
    public int? LinkedAccountId { get; set; }

    public virtual Person? Person { get; set; }
    public virtual Account? LinkedAccount { get; set; }
    public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
}
