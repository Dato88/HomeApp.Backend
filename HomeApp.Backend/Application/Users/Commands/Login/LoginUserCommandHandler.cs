using Application.Abstractions.Authentication;
using Application.Abstractions.Logging;
using Application.Abstractions.Messaging;
using Application.Email;
using Application.Models.Email;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Application.Users.Commands.Login;

internal sealed class LoginUserCommandHandler(
    ITokenProvider tokenProvider,
    UserManager<User> userManager,
    IEmailSender emailSender,
    IAppLogger<LoginUserCommandHandler> logger) : ICommandHandler<LoginUserCommand, AuthResponse>
{
    public async Task<Result<AuthResponse>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);

        if (user is null)
        {
            logger.LogWarning($"Login attempt failed. No user found with email: {command.Email}");

            return Result.Failure<AuthResponse>(UserErrors.Unauthorized());
        }

        if (!await userManager.IsEmailConfirmedAsync(user))
        {
            logger.LogWarning($"Login attempt with unconfirmed email: {command.Email}");

            return Result.Failure<AuthResponse>(UserErrors.EmailNotConfirmed);
        }

        if (!await userManager.CheckPasswordAsync(user, command.Password))
        {
            await userManager.AccessFailedAsync(user);

            if (!await userManager.IsLockedOutAsync(user))
            {
                logger.LogWarning($"Invalid password attempt for user {command.Email}");

                return Result.Failure<AuthResponse>(UserErrors.Unauthorized());
            }

            var content =
                $"Your account is locked out. To reset the password click this link: {command.ClientUri}";
            var message = new Message(new[] { command.Email }, "Locked out account information", content);

            await emailSender.SendEmailAsync(message, cancellationToken);

            logger.LogError($"User {command.Email} is locked out due to multiple failed login attempts.");

            return Result.Failure<AuthResponse>(UserErrors.LockedOut(new Guid(user.Id)));
        }

        if (await userManager.GetTwoFactorEnabledAsync(user))
        {
            logger.LogInformation($"User {command.Email} requires 2FA. Sending verification email...");

            return await GenerateOtpFor2StepVerification(user, cancellationToken);
        }

        var token = await tokenProvider.Create(user);

        await userManager.ResetAccessFailedCountAsync(user);

        logger.LogInformation($"User {command.Email} successfully logged in.");

        return Result.Success(new AuthResponse { Token = token });
    }

    private async Task<Result<AuthResponse>> GenerateOtpFor2StepVerification(User user,
        CancellationToken cancellationToken)
    {
        var providers = await userManager.GetValidTwoFactorProvidersAsync(user);

        if (!providers.Contains("Email"))
        {
            logger.LogWarning($"User {user.Email} has no valid 2FA email provider.");

            return Result.Failure<AuthResponse>(UserErrors.Invalid2Step(new Guid(user.Id)));
        }

        var token = await userManager.GenerateTwoFactorTokenAsync(user, "Email");
        var message = new Message(new[] { user.Email }, "Authentication token", token);

        await emailSender.SendEmailAsync(message, cancellationToken);

        logger.LogInformation($"2FA email token sent to user {user.Email}.");

        return Result.Success(new AuthResponse { Is2StepVerificationRequired = true, Provider = "Email" });
    }
}
