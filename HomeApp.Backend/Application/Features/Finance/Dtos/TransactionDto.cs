using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;

namespace Application.Features.Finance.Dtos;

public sealed class TransactionDto
{
    public int TransactionId { get; set; }
    public int AccountId { get; set; }
    public DateOnly BookingDate { get; set; }
    public DateOnly? ValueDate { get; set; }
    public decimal Amount { get; set; }
    public string? PaymentPartnerName { get; set; }
    public string? PaymentPartnerIban { get; set; }
    public string? Purpose { get; set; }
    public string? BankReference { get; set; }
    public int? CategoryId { get; set; }
    public int? PaymentPartnerId { get; set; }
    public TransactionSource Source { get; set; }

    public static explicit operator TransactionDto(Transaction entity) =>
        new()
        {
            TransactionId = entity.TransactionId,
            AccountId = entity.AccountId,
            BookingDate = entity.BookingDate,
            ValueDate = entity.ValueDate,
            Amount = entity.Amount,
            PaymentPartnerName = entity.PaymentPartnerName,
            PaymentPartnerIban = entity.PaymentPartnerIban,
            Purpose = entity.Purpose,
            BankReference = entity.BankReference,
            CategoryId = entity.CategoryId,
            PaymentPartnerId = entity.PaymentPartnerId,
            Source = entity.Source
        };
}
