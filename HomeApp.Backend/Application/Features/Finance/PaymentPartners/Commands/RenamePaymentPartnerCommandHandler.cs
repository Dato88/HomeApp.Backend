using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.PaymentPartners.Commands;

public sealed class RenamePaymentPartnerCommandHandler(
    IPaymentPartnerCommands paymentPartnerCommands,
    IAppLogger<RenamePaymentPartnerCommandHandler> logger)
    : IRequestHandler<RenamePaymentPartnerCommand, Result<int>>
{
    private readonly IPaymentPartnerCommands _paymentPartnerCommands = paymentPartnerCommands;
    private readonly IAppLogger<RenamePaymentPartnerCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(RenamePaymentPartnerCommand request, CancellationToken cancellationToken)
    {
        var result = await _paymentPartnerCommands.RenamePaymentPartnerAsync(request.PaymentPartnerId,
            request.DisplayName, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning(
                $"Renaming payment partner failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Renaming payment partner: {result.Value}");

        return result;
    }
}
