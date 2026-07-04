using Application.Features.Finance.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Accounts.Queries;

public sealed record GetAccountsQuery : IRequest<Result<List<AccountDto>>>;
