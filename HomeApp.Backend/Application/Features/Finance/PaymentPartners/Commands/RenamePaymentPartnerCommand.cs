using MediatR;
using SharedKernel;

namespace Application.Features.Finance.PaymentPartners.Commands;

public sealed record RenamePaymentPartnerCommand(
    int PaymentPartnerId,
    string DisplayName) : IRequest<Result<int>>;
