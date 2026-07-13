using MediatR;
using SharedKernel;

namespace Application.Features.Finance.PaymentPartners.Commands;

public sealed record MergePaymentPartnersCommand(
    int TargetPaymentPartnerId,
    int SourcePaymentPartnerId) : IRequest<Result<int>>;
