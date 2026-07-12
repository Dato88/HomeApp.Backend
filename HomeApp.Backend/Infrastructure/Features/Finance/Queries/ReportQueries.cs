using Application.Abstractions.Authentication;
using Application.Abstractions.FinanceModule;
using Domain.Entities.Finance;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Features.Finance.Queries;

public sealed class ReportQueries(HomeAppContext dbContext, IExecutionContextAccessor executionContext)
    : IReportQueries
{
    private readonly HomeAppContext _dbContext = dbContext;
    private readonly IExecutionContextAccessor _executionContext = executionContext;

    public async Task<Result<EvaReportData>> GetEvaReportDataAsync(IReadOnlyList<int> householdIds, int year,
        CancellationToken cancellationToken)
    {
        var ids = householdIds.Distinct().ToList();

        // The caller must be a member of every requested household; a silent partial report over
        // only the accessible households would be misleading
        var memberCount = await _dbContext.HouseholdMembers
            .CountAsync(m => ids.Contains(m.HouseholdId) && m.PersonId == _executionContext.PersonId,
                cancellationToken);

        if (memberCount != ids.Count)
            return Result.Failure<EvaReportData>(FinanceErrors.ReportFailedWithMessage("HouseholdId is invalid"));

        var groups = await _dbContext.CategoryGroups
            .AsNoTracking()
            .Where(g => ids.Contains(g.HouseholdId))
            .OrderBy(g => g.Name)
            .ToListAsync(cancellationToken);

        var categories = await _dbContext.Categories
            .AsNoTracking()
            .Where(c => ids.Contains(c.HouseholdId))
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);

        // IST source: one GROUP BY over all transactions of accounts shared into the selected
        // households, aggregated per category and month. The EXISTS filter counts every transaction
        // exactly once even when its account is shared into several of the selected households.
        var totals = await _dbContext.Transactions
            .AsNoTracking()
            .Where(t =>
                t.BookingDate.Year == year &&
                t.Account.AccountHouseholds.Any(ah => ids.Contains(ah.HouseholdId)))
            .GroupBy(t => new { t.CategoryId, t.BookingDate.Month })
            .Select(g => new CategoryMonthAmount(g.Key.CategoryId, g.Key.Month, g.Sum(t => t.Amount), g.Count()))
            .ToListAsync(cancellationToken);

        return Result.Success(new EvaReportData(groups, categories, totals));
    }
}
