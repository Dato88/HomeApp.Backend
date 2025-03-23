using Application.Models.Email;

namespace Application.Email;

public interface IEmailSender
{
    Task SendEmailAsync(Message message, CancellationToken cancellationToken);
}
