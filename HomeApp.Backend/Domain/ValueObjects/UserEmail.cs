using System.Net.Mail;

namespace Domain.ValueObjects;

public readonly record struct UserEmail
{
    public UserEmail(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"'{nameof(value)}' cannot be null or whitespace.", nameof(value));

        if (!IsValidEmail(value))
            throw new ArgumentException($"'{nameof(value)}' is not a valid email address.", nameof(value));

        Value = value;
    }

    public string Value { get; }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var mailAdress = new MailAddress(email);

            return mailAdress.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
