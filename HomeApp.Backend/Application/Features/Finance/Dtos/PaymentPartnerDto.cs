using Application.Abstractions.FinanceModule;

namespace Application.Features.Finance.Dtos;

public sealed class PaymentPartnerDto
{
    public int PaymentPartnerId { get; set; }
    public string DisplayName { get; set; } = default!;
    public string? Iban { get; set; }
    public int? LinkedAccountId { get; set; }
    public int TransactionCount { get; set; }
    public DateOnly? LastBookingDate { get; set; }

    public static explicit operator PaymentPartnerDto(PaymentPartnerWithStats item) =>
        new()
        {
            PaymentPartnerId = item.Partner.PaymentPartnerId,
            DisplayName = item.Partner.DisplayName,
            Iban = item.Partner.Iban,
            LinkedAccountId = item.Partner.LinkedAccountId,
            TransactionCount = item.TransactionCount,
            LastBookingDate = item.LastBookingDate
        };
}
