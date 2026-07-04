using Application.Abstractions.Authentication;
using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using Application.Features.Finance.Dtos;
using Domain.Entities.Finance;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Accounts.Queries;

public sealed class GetAccountsQueryHandler(
    IAccountQueries accountQueries,
    IExecutionContextAccessor executionContext,
    IAppLogger<GetAccountsQueryHandler> logger)
    : IRequestHandler<GetAccountsQuery, Result<List<AccountDto>>>
{
    private readonly IAccountQueries _accountQueries = accountQueries;
    private readonly IExecutionContextAccessor _executionContext = executionContext;
    private readonly IAppLogger<GetAccountsQueryHandler> _logger = logger;

    public async Task<Result<List<AccountDto>>> Handle(GetAccountsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _accountQueries.GetAccountsAsync(cancellationToken);

            if (result.IsFailure)
                return Result.Failure<List<AccountDto>>(result.Error);

            var response = result.Value.Select(a =>
            {
                var dto = (AccountDto)a;
                dto.IsOwner = a.PersonId == _executionContext.PersonId;
                return dto;
            }).ToList();

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Get accounts failed: {ex}");

            return Result.Failure<List<AccountDto>>(FinanceErrors.UnexpectedError(ex.Message));
        }
    }
}
