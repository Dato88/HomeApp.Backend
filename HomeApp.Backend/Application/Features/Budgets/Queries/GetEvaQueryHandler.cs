using Application.Abstractions.BudgetModule;
using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using Application.Features.Budgets.DTOs.Eva;
using Domain.Entities.Budgets;
using Domain.Entities.Budgets.Enums;
using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Queries;

public sealed class GetEvaQueryHandler(
    IBudgetQueries budgetQueries,
    ITransactionQueries transactionQueries,
    IAppLogger<GetEvaQueryHandler> logger)
    : IRequestHandler<GetEvaQuery, Result<EvaResponse>>
{
    private readonly IBudgetQueries _budgetQueries = budgetQueries;
    private readonly ITransactionQueries _transactionQueries = transactionQueries;
    private readonly IAppLogger<GetEvaQueryHandler> _logger = logger;

    public async Task<Result<EvaResponse>> Handle(GetEvaQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var budgetResult = await _budgetQueries.GetBudgetAsync(request.HouseholdId, request.Year,
                cancellationToken);

            if (budgetResult.IsFailure)
                return Result.Failure<EvaResponse>(BudgetErrors.NotFoundAll);

            var totalsResult = await _transactionQueries.GetMonthlyCategoryTotalsAsync(request.HouseholdId,
                request.Year, cancellationToken);

            if (totalsResult.IsFailure)
                return Result.Failure<EvaResponse>(totalsResult.Error);

            var response = BuildResponse(request.HouseholdId, budgetResult.Value, totalsResult.Value);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Get eva failed: {ex}");

            return Result.Failure<EvaResponse>(BudgetErrors.UnexpectedError(ex.Message));
        }
    }

    private static EvaResponse BuildResponse(int householdId, Budget budget,
        IReadOnlyList<CategoryMonthAmount> totals)
    {
        // Raw (signed) IST per category and month
        var istByCategory = new Dictionary<int, decimal[]>();

        foreach (var total in totals.Where(t => t.CategoryId.HasValue))
        {
            if (!istByCategory.TryGetValue(total.CategoryId!.Value, out var amounts))
                istByCategory[total.CategoryId.Value] = amounts = new decimal[12];

            amounts[total.Month - 1] += total.Sum;
        }

        var incomeSoll = new decimal[12];
        var incomeIst = new decimal[12];
        var expenseSoll = new decimal[12];
        var expenseIst = new decimal[12];
        var mappedCategoryIds = new HashSet<int>();
        var groupBuffer = new List<(BudgetGroup Group, List<EvaRowDto> Rows, decimal[] Soll, decimal[] Ist)>();

        foreach (var group in budget.BudgetGroups.OrderBy(g => g.Index))
        {
            var groupSoll = new decimal[12];
            var groupIst = new decimal[12];
            var rows = new List<EvaRowDto>();

            foreach (var row in group.BudgetRows.OrderBy(r => r.Index))
            {
                var soll = new decimal[12];

                foreach (var cell in row.BudgetCells)
                    soll[cell.Month - 1] += cell.Amount;

                // IST is displayed positive for both group types (like the Excel sheet)
                var ist = new decimal[12];

                if (row.CategoryId.HasValue)
                {
                    mappedCategoryIds.Add(row.CategoryId.Value);

                    if (istByCategory.TryGetValue(row.CategoryId.Value, out var raw))
                    {
                        for (var month = 0; month < 12; month++)
                            ist[month] = group.BudgetGroupType == BudgetGroupType.Expense
                                ? -raw[month]
                                : raw[month];
                    }
                }

                var diff = new decimal[12];

                for (var month = 0; month < 12; month++)
                {
                    diff[month] = soll[month] - ist[month];
                    groupSoll[month] += soll[month];
                    groupIst[month] += ist[month];
                }

                rows.Add(new EvaRowDto(row.BudgetRowId, row.Index, row.Title, row.CategoryId,
                    row.Category?.Name, soll, ist, diff, soll.Sum(), ist.Sum()));
            }

            for (var month = 0; month < 12; month++)
            {
                if (group.BudgetGroupType == BudgetGroupType.Income)
                {
                    incomeSoll[month] += groupSoll[month];
                    incomeIst[month] += groupIst[month];
                }
                else if (group.BudgetGroupType == BudgetGroupType.Expense)
                {
                    expenseSoll[month] += groupSoll[month];
                    expenseIst[month] += groupIst[month];
                }
            }

            groupBuffer.Add((group, rows, groupSoll, groupIst));
        }

        var yearIncomeSoll = incomeSoll.Sum();
        var yearIncomeIst = incomeIst.Sum();

        var groups = new List<EvaGroupDto>();

        foreach (var (group, rows, groupSoll, groupIst) in groupBuffer)
        {
            var groupDiff = new decimal[12];

            for (var month = 0; month < 12; month++)
                groupDiff[month] = groupSoll[month] - groupIst[month];

            var yearSoll = groupSoll.Sum();
            var yearIst = groupIst.Sum();

            groups.Add(new EvaGroupDto(group.BudgetGroupId, group.Index, group.Title, group.BudgetGroupType,
                group.TargetPercent,
                Percent(yearSoll, yearIncomeSoll),
                Percent(yearIst, yearIncomeIst),
                rows, groupSoll, groupIst, groupDiff, yearSoll, yearIst));
        }

        var diffSoll = new decimal[12];
        var diffIst = new decimal[12];

        for (var month = 0; month < 12; month++)
        {
            diffSoll[month] = incomeSoll[month] - expenseSoll[month];
            diffIst[month] = incomeIst[month] - expenseIst[month];
        }

        var totalsDto = new EvaTotalsDto(incomeSoll, incomeIst, expenseSoll, expenseIst, diffSoll, diffIst,
            yearIncomeSoll, yearIncomeIst, expenseSoll.Sum(), expenseIst.Sum(),
            yearIncomeSoll - expenseSoll.Sum(), yearIncomeIst - expenseIst.Sum());

        // Everything not mapped to a row of this budget (uncategorized or unmapped category), signed
        var unassignedIst = new decimal[12];
        var unassignedCount = 0;

        foreach (var total in totals)
        {
            if (total.CategoryId.HasValue && mappedCategoryIds.Contains(total.CategoryId.Value))
                continue;

            unassignedIst[total.Month - 1] += total.Sum;
            unassignedCount += total.Count;
        }

        return new EvaResponse(budget.BudgetId, householdId, budget.Year, groups, totalsDto, unassignedIst,
            unassignedCount);
    }

    private static decimal? Percent(decimal value, decimal baseValue) =>
        baseValue == 0 ? null : Math.Round(value / baseValue * 100, 2);
}
