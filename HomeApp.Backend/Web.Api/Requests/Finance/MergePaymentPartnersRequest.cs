using Application.Features.Finance.PaymentPartners.Commands;

namespace Web.Api.Requests.Finance;

public sealed record MergePaymentPartnersRequest
{
    public int TargetPaymentPartnerId { get; init; }
    public int SourcePaymentPartnerId { get; init; }

    public static explicit operator MergePaymentPartnersCommand(MergePaymentPartnersRequest request)
        => new(request.TargetPaymentPartnerId, request.SourcePaymentPartnerId);
}
