using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands.Create;

public sealed record CreateBudgetCommand(int HouseholdId, int Year) : IRequest<Result<int>>;
