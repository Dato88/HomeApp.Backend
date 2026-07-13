using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using Application.Features.Finance.Dtos;
using Domain.Entities.Finance;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.PaymentPartners.Queries;

public sealed class GetPaymentPartnersQueryHandler(
    IPaymentPartnerQueries paymentPartnerQueries,
    IAppLogger<GetPaymentPartnersQueryHandler> logger)
    : IRequestHandler<GetPaymentPartnersQuery, Result<List<PaymentPartnerDto>>>
{
    private readonly IPaymentPartnerQueries _paymentPartnerQueries = paymentPartnerQueries;
    private readonly IAppLogger<GetPaymentPartnersQueryHandler> _logger = logger;

    public async Task<Result<List<PaymentPartnerDto>>> Handle(GetPaymentPartnersQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _paymentPartnerQueries.GetPaymentPartnersAsync(cancellationToken);

            if (result.IsFailure)
                return Result.Failure<List<PaymentPartnerDto>>(result.Error);

            var response = result.Value.Select(p => (PaymentPartnerDto)p).ToList();

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Get payment partners failed: {ex}");

            return Result.Failure<List<PaymentPartnerDto>>(FinanceErrors.UnexpectedError(ex.Message));
        }
    }
}
