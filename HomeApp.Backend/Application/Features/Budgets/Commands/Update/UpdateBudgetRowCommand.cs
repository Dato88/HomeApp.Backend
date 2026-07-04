using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands.Update;

public sealed record UpdateBudgetRowCommand(
    int BudgetRowId,
    int Index,
    string Title,
    int? CategoryId = null) : IRequest<Result<int>>;
