using System.Security.Authentication;
using Application.Models.Email;
using HomeApp.Library.Logger;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;

namespace Application.Email;

public class EmailSender(EmailConfiguration emailConfig, ILogger<EmailSender> logger)
    : LoggerExtension<EmailSender>(logger), IEmailSender
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
            LogError($"Authentication failed: {ex.Message}", DateTime.UtcNow);
            throw;
        }
        catch (SmtpCommandException ex)
        {
            LogError($"SMTP command error: {ex.Message}", DateTime.UtcNow);
            throw;
        }
        catch (Exception ex)
        {
            LogException($"Unexpected error: {ex.Message}", DateTime.UtcNow);
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true, cancellationToken);
            LogInformation("Disconnected from the SMTP server.", DateTime.UtcNow);

            client.Dispose();
        }
    }
}
