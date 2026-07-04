using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands.Update;

public sealed record UpdateBudgetCellCommand(
    int BudgetCellId,
    decimal Amount) : IRequest<Result<int>>;
