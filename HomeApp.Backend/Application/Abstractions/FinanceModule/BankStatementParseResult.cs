namespace Application.Abstractions.FinanceModule;

public sealed record BankStatementParseResult(
    IReadOnlyList<ParsedTransaction> Transactions,
    IReadOnlyList<string> Errors);
