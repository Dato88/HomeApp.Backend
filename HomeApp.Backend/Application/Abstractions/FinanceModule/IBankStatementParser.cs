using Domain.Entities.Finance.Enums;
using SharedKernel;

namespace Application.Abstractions.FinanceModule;

public interface IBankStatementParser
{
    string FormatName { get; }
    TransactionSource Source { get; }

    bool CanParse(string fileName, byte[] head);
    Result<BankStatementParseResult> Parse(Stream content);
}
