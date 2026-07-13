using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.PaymentPartners.Commands;

public sealed class MergePaymentPartnersCommandHandler(
    IPaymentPartnerCommands paymentPartnerCommands,
    IAppLogger<MergePaymentPartnersCommandHandler> logger)
    : IRequestHandler<MergePaymentPartnersCommand, Result<int>>
{
    private readonly IPaymentPartnerCommands _paymentPartnerCommands = paymentPartnerCommands;
    private readonly IAppLogger<MergePaymentPartnersCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(MergePaymentPartnersCommand request, CancellationToken cancellationToken)
    {
        var result = await _paymentPartnerCommands.MergePaymentPartnersAsync(request.TargetPaymentPartnerId,
            request.SourcePaymentPartnerId, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning(
                $"Merging payment partners failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation(
            $"Merged payment partner {request.SourcePaymentPartnerId} into {request.TargetPaymentPartnerId}, " +
            $"moved {result.Value} transactions");

        return result;
    }
}
