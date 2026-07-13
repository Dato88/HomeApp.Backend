using Application.Features.Finance.PaymentPartners.Commands;

namespace Web.Api.Requests.Finance;

public sealed record RenamePaymentPartnerRequest
{
    public int PaymentPartnerId { get; init; }
    public string DisplayName { get; init; } = string.Empty;

    public static explicit operator RenamePaymentPartnerCommand(RenamePaymentPartnerRequest request)
        => new(request.PaymentPartnerId, request.DisplayName);
}
