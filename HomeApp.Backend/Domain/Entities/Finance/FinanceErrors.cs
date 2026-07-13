using SharedKernel;

namespace Domain.Entities.Finance;

public static class FinanceErrors
{
    public static Error AccountCreateFailedWithMessage(string message) => Error.Failure(
        "Finance.AccountCreateFailedWithMessage",
        $"The account could not be created with message = '{message}'");

    public static Error AccountUpdateFailedWithMessage(string message) => Error.Failure(
        "Finance.AccountUpdateFailedWithMessage",
        $"The account could not be updated with message = '{message}'");

    public static Error AccountDeleteFailedWithMessage(string message) => Error.Failure(
        "Finance.AccountDeleteFailedWithMessage",
        $"The account could not be deleted with message = '{message}'");

    public static Error AccountShareFailedWithMessage(string message) => Error.Failure(
        "Finance.AccountShareFailedWithMessage",
        $"The account could not be shared with message = '{message}'");

    public static Error CategoryGroupCreateFailedWithMessage(string message) => Error.Failure(
        "Finance.CategoryGroupCreateFailedWithMessage",
        $"The category group could not be created with message = '{message}'");

    public static Error CategoryGroupUpdateFailedWithMessage(string message) => Error.Failure(
        "Finance.CategoryGroupUpdateFailedWithMessage",
        $"The category group could not be updated with message = '{message}'");

    public static Error CategoryGroupDeleteFailedWithMessage(string message) => Error.Failure(
        "Finance.CategoryGroupDeleteFailedWithMessage",
        $"The category group could not be deleted with message = '{message}'");

    public static Error ReportFailedWithMessage(string message) => Error.Failure(
        "Finance.ReportFailedWithMessage",
        $"The report could not be created with message = '{message}'");

    public static Error CategoryCreateFailedWithMessage(string message) => Error.Failure(
        "Finance.CategoryCreateFailedWithMessage",
        $"The category could not be created with message = '{message}'");

    public static Error CategoryUpdateFailedWithMessage(string message) => Error.Failure(
        "Finance.CategoryUpdateFailedWithMessage",
        $"The category could not be updated with message = '{message}'");

    public static Error CategoryDeleteFailedWithMessage(string message) => Error.Failure(
        "Finance.CategoryDeleteFailedWithMessage",
        $"The category could not be deleted with message = '{message}'");

    public static Error TransactionCreateFailedWithMessage(string message) => Error.Failure(
        "Finance.TransactionCreateFailedWithMessage",
        $"The transaction could not be created with message = '{message}'");

    public static Error TransactionUpdateFailedWithMessage(string message) => Error.Failure(
        "Finance.TransactionUpdateFailedWithMessage",
        $"The transaction could not be updated with message = '{message}'");

    public static Error TransactionDeleteFailedWithMessage(string message) => Error.Failure(
        "Finance.TransactionDeleteFailedWithMessage",
        $"The transaction could not be deleted with message = '{message}'");

    public static Error ImportFailedWithMessage(string message) => Error.Failure(
        "Finance.ImportFailedWithMessage",
        $"The import failed with message = '{message}'");

    public static Error PaymentPartnerUpdateFailedWithMessage(string message) => Error.Failure(
        "Finance.PaymentPartnerUpdateFailedWithMessage",
        $"The payment partner could not be updated with message = '{message}'");

    public static Error PaymentPartnerMergeFailedWithMessage(string message) => Error.Failure(
        "Finance.PaymentPartnerMergeFailedWithMessage",
        $"The payment partners could not be merged with message = '{message}'");

    public static Error UnexpectedError(string message) => Error.Failure(
        "Finance.UnexpectedError",
        $"An unexpected error occurred: {message}");
}
