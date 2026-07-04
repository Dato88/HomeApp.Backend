using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Accounts.Commands;

public sealed record UnshareAccountCommand(int AccountId, int HouseholdId) : IRequest<Result<int>>;
