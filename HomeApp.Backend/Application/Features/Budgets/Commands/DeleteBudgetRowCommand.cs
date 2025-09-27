using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands;

public sealed record DeleteBudgetRowCommand(int BudgetRowId) : IRequest<Result<int>>;
