using FluentValidation;

namespace Application.Features.Finance.Transactions.Commands;

internal sealed class ImportTransactionsCommandValidator : AbstractValidator<ImportTransactionsCommand>
{
    private static readonly string[] AllowedExtensions = [".csv", ".xml", ".txt"];

    public ImportTransactionsCommandValidator()
    {
        RuleFor(c => c.AccountId).GreaterThan(0);

        RuleFor(c => c.FileName).NotEmpty()
            .Must(fileName => AllowedExtensions.Contains(Path.GetExtension(fileName),
                StringComparer.OrdinalIgnoreCase))
            .WithMessage("Only .csv, .xml and .txt files are supported.");

        RuleFor(c => c.Content).NotEmpty()
            .WithMessage("File is empty.");
    }
}
