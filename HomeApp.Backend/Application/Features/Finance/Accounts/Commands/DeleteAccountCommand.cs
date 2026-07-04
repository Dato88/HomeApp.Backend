using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Accounts.Commands;

public sealed record DeleteAccountCommand(int AccountId) : IRequest<Result<int>>;
