using MediatR;
using SharedKernel;

namespace Application.Features.Households.Commands;

public sealed record RemoveHouseholdMemberCommand(int HouseholdId, int PersonId) : IRequest<Result<int>>;
