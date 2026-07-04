using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using Application.Features.Finance.Dtos;
using Domain.Entities.Finance;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Transactions.Commands;

public sealed class ImportTransactionsCommandHandler(
    ITransactionCommands transactionCommands,
    IEnumerable<IBankStatementParser> parsers,
    IAppLogger<ImportTransactionsCommandHandler> logger)
    : IRequestHandler<ImportTransactionsCommand, Result<ImportTransactionsResponse>>
{
    private readonly ITransactionCommands _transactionCommands = transactionCommands;
    private readonly IEnumerable<IBankStatementParser> _parsers = parsers;
    private readonly IAppLogger<ImportTransactionsCommandHandler> _logger = logger;

    public async Task<Result<ImportTransactionsResponse>> Handle(ImportTransactionsCommand request,
        CancellationToken cancellationToken)
    {
        var parser = ResolveParser(request);

        if (parser is null)
        {
            _logger.LogWarning($"No import parser found for file '{request.FileName}'");

            return Result.Failure<ImportTransactionsResponse>(
                FinanceErrors.ImportFailedWithMessage("No parser found for this file format"));
        }

        using var stream = new MemoryStream(request.Content);
        var parseResult = parser.Parse(stream);

        if (parseResult.IsFailure)
        {
            _logger.LogWarning($"Import parse failed: {parseResult.Error.Description}");

            return Result.Failure<ImportTransactionsResponse>(parseResult.Error);
        }

        var importResult = await _transactionCommands.ImportTransactionsAsync(request.AccountId,
            parseResult.Value.Transactions, parser.Source, cancellationToken);

        if (importResult.IsFailure)
        {
            _logger.LogWarning($"Import failed: {importResult.Error.Description} ({importResult.Error.Code})");

            return Result.Failure<ImportTransactionsResponse>(importResult.Error);
        }

        _logger.LogInformation(
            $"Import ({parser.FormatName}): {importResult.Value.Imported} imported, " +
            $"{importResult.Value.SkippedDuplicates} duplicates, {parseResult.Value.Errors.Count} failed rows");

        return Result.Success(new ImportTransactionsResponse(
            importResult.Value.Imported,
            importResult.Value.SkippedDuplicates,
            parseResult.Value.Errors.Count,
            parseResult.Value.Errors));
    }

    private IBankStatementParser? ResolveParser(ImportTransactionsCommand request)
    {
        if (!string.IsNullOrWhiteSpace(request.Format))
            return _parsers.FirstOrDefault(p =>
                string.Equals(p.FormatName, request.Format, StringComparison.OrdinalIgnoreCase));

        var head = request.Content.Take(512).ToArray();
        return _parsers.FirstOrDefault(p => p.CanParse(request.FileName, head));
    }
}
