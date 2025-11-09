using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands.Delete;

public sealed record DeleteBudgetCellCommand(int BudgetCellId) : IRequest<Result<int>>;
