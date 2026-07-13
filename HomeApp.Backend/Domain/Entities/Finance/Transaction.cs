using Domain.Entities.Finance.Enums;
using SharedKernel;

namespace Domain.Entities.Finance;

public class Transaction : AuditableEntity
{
    public int TransactionId { get; set; }

    public int AccountId { get; set; }
    public DateOnly BookingDate { get; set; }
    public DateOnly? ValueDate { get; set; }

    // Signed amount: negative = expense, positive = income
    public decimal Amount { get; set; }

    // Raw partner strings as delivered by the bank export (audit trail);
    // the deduplicated partner is referenced via PaymentPartnerId.
    public string? PaymentPartnerName { get; set; }
    public string? PaymentPartnerIban { get; set; }
    public string? Purpose { get; set; }
    public string? BankReference { get; set; }
    public int? CategoryId { get; set; }
    public int? PaymentPartnerId { get; set; }
    public string? ImportHash { get; set; }
    public TransactionSource Source { get; set; }

    public virtual Account? Account { get; set; }
    public virtual Category? Category { get; set; }
    public virtual PaymentPartner? PaymentPartner { get; set; }
}
