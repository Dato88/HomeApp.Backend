using SharedKernel;

namespace Application.Abstractions.FinanceModule;

public interface IPaymentPartnerQueries
{
    Task<Result<List<PaymentPartnerWithStats>>> GetPaymentPartnersAsync(CancellationToken cancellationToken);
}
