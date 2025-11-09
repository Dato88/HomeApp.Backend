using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands.Create;

public sealed record CreateBudgetCommand(int Year) : IRequest<Result<int>>;
