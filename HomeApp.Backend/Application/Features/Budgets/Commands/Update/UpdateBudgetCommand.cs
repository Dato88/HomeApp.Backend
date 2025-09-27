using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands.Update;

public sealed record UpdateBudgetCommand(int BudgetId, int Year) : IRequest<Result<int>>;
