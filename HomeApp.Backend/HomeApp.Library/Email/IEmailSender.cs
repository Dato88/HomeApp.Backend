using HomeApp.Library.Models.Email;

namespace HomeApp.Library.Email;

public interface IEmailSender
{
    Task SendEmailAsync(Message message, CancellationToken cancellationToken);
}
