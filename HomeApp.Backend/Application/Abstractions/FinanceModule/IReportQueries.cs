using SharedKernel;

namespace Application.Abstractions.FinanceModule;

public interface IReportQueries
{
    Task<Result<EvaReportData>> GetEvaReportDataAsync(IReadOnlyList<int> householdIds, int year,
        CancellationToken cancellationToken);
}
