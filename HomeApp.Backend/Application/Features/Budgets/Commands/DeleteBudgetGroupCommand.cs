using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands;

public sealed record DeleteBudgetGroupCommand(int BudgetGroupId) : IRequest<Result<int>>;
