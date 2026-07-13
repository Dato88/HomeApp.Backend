using SharedKernel;

namespace Application.Abstractions.FinanceModule;

public interface IPaymentPartnerCommands
{
    Task<Result<int>> RenamePaymentPartnerAsync(int paymentPartnerId, string displayName,
        CancellationToken cancellationToken);

    Task<Result<int>> MergePaymentPartnersAsync(int targetPaymentPartnerId, int sourcePaymentPartnerId,
        CancellationToken cancellationToken);
}
