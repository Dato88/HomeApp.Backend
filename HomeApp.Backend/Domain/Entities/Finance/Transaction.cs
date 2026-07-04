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
    public string? CounterpartyName { get; set; }
    public string? CounterpartyIban { get; set; }
    public string? Purpose { get; set; }
    public string? BankReference { get; set; }
    public int? CategoryId { get; set; }
    public string? ImportHash { get; set; }
    public TransactionSource Source { get; set; }

    public virtual Account? Account { get; set; }
    public virtual Category? Category { get; set; }
}
