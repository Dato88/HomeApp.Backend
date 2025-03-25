using System.Security.Authentication;
using Application.Abstractions.Logging;
using Application.Models.Email;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace Application.Email;

public class EmailSender(EmailConfiguration emailConfig, IAppLogger<EmailSender> logger)
    : IEmailSender
{
    public async Task SendEmailAsync(Message message, CancellationToken cancellationToken)
    {
        var emailMessage = CreateEmailMessage(message);
        await SendAsync(emailMessage, cancellationToken);
    }

    private MimeMessage CreateEmailMessage(Message message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("email", emailConfig.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(TextFormat.Text) { Text = message.Content };

        return emailMessage;
    }

    private async Task SendAsync(MimeMessage mailMessage, CancellationToken cancellationToken)
    {
        using var client = new SmtpClient();

        try
        {
            await client.ConnectAsync(emailConfig.SmtpServer, emailConfig.Port, true, cancellationToken);
            client.AuthenticationMechanisms.Remove("XOAUTH2");

            await client.AuthenticateAsync(emailConfig.UserName, emailConfig.Password, cancellationToken);
            await client.SendAsync(mailMessage, cancellationToken);
        }
        catch (AuthenticationException ex)
        {
            logger.LogError($"Authentication failed: {ex.Message}");
            throw;
        }
        catch (SmtpCommandException ex)
        {
            logger.LogError($"SMTP command error: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogException($"Unexpected error: {ex.Message}");
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true, cancellationToken);
            logger.LogInformation("Disconnected from the SMTP server.");

            client.Dispose();
        }
    }
}
