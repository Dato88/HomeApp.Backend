using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands;

public sealed record DeleteBudgetCommand(int BudgetId) : IRequest<Result<int>>;
