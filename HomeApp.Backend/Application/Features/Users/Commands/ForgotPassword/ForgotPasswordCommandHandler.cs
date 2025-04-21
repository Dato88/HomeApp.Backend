using Application.Abstractions.Logging;
using Application.Abstractions.Messaging;
using Application.Email;
using Application.Models.Email;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using SharedKernel;

namespace Application.Features.Users.Commands.ForgotPassword;

internal sealed class ForgotPasswordCommandHandler(
    IEmailSender emailSender,
    UserManager<User> userManager,
    IAppLogger<ForgotPasswordCommandHandler> logger)
    : ICommandHandler<ForgotPasswordCommand, Guid>
{
    public async Task<Result<Guid>> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);

        if (user == null)
        {
            logger.LogWarning($"Password reset requested for unknown email: {command.Email}");

            return Result.Failure<Guid>(UserErrors.NotFoundByEmail);
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var param = new Dictionary<string, string?> { { "token", token }, { "email", command.Email } };

        var callback = QueryHelpers.AddQueryString(command.ClientUri, param);

        var message = new Message(new[] { user.Email }, "Reset password token", callback);

        await emailSender.SendEmailAsync(message, cancellationToken);

        logger.LogInformation($"Password reset email sent to {user.Email} (UserId: {user.Id})");

        return Result.Success(new Guid(user.Id));
    }
}
