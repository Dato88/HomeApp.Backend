using Application.Features.Finance.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Transactions.Commands;

public sealed record ImportTransactionsCommand(
    int AccountId,
    string FileName,
    byte[] Content,
    string? Format) : IRequest<Result<ImportTransactionsResponse>>;
