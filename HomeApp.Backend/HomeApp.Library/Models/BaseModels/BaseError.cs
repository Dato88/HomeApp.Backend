namespace HomeApp.Library.Models.BaseModels;

public class BaseError(string? innerExceptionMessage, string? errorMessage)
{
    public string? InnerExceptionMessage { get; set; } = innerExceptionMessage;
    public string? ErrorMessage { get; set; } = errorMessage;

    public static implicit operator BaseError(Exception item) =>
        new(item.InnerException?.Message, item.Message);
}
