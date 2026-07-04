using Domain.Entities.Budgets.Enums;
using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands.Update;

public sealed record UpdateBudgetGroupCommand(
    int BudgetGroupId,
    int Index,
    string Title,
    BudgetGroupType BudgetGroupType,
    decimal? TargetPercent = null) : IRequest<Result<int>>;
