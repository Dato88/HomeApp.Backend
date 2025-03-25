using Application.Abstractions.Messaging;
using Application.Email;
using Application.Models.Email;
using Domain.Entities.User;
using Infrastructure.Logger;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.Users.Commands.ForgotPassword;

internal sealed class ForgotPasswordCommandHandler(
    IEmailSender emailSender,
    UserManager<User> userManager,
    ILogger<ForgotPasswordCommandHandler> logger)
    : ICommandHandler<ForgotPasswordCommand, Guid>
{
    private readonly LoggerExtension<ForgotPasswordCommandHandler> _logger = new(logger);

    public async Task<Result<Guid>> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);

        if (user == null)
        {
            _logger.LogInformation("Email is not found", DateTime.Now);

            return Result.Failure<Guid>(UserErrors.NotFoundByEmail);
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var param = new Dictionary<string, string?> { { "token", token }, { "email", command.Email } };

        var callback = QueryHelpers.AddQueryString(command.ClientUri, param);

        var message = new Message(new[] { user.Email }, "Reset password token", callback);

        await emailSender.SendEmailAsync(message, cancellationToken);

        _logger.LogInformation($"Forgot password E-mail is being sent by {user.Id}", DateTime.Now);

        return Result.Success(new Guid(user.Id));
    }
}
