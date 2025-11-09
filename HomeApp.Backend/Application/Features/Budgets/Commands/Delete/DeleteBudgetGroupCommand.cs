using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands.Delete;

public sealed record DeleteBudgetGroupCommand(int BudgetGroupId) : IRequest<Result<int>>;
