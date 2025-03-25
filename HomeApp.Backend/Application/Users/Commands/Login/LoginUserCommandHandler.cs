using Application.Abstractions.Messaging;
using Application.Email;
using Application.Models.Email;
using Domain.Entities.User;
using Infrastructure.Authorization.Handler;
using Infrastructure.Logger;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.Users.Commands.Login;

internal sealed class LoginUserCommandHandler(
    JwtHandler jwtHandler,
    UserManager<User> userManager,
    IEmailSender emailSender,
    ILogger<LoginUserCommandHandler> logger) : ICommandHandler<LoginUserCommand, AuthResponse>
{
    private readonly LoggerExtension<LoginUserCommandHandler> _logger = new(logger);

    public async Task<Result<AuthResponse>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);

        if (!await userManager.IsEmailConfirmedAsync(user))
        {
            _logger.LogInformation("Email is not confirmed", DateTime.Now);

            return Result.Failure<AuthResponse>(UserErrors.EmailNotConfirmed);
        }

        if (!await userManager.CheckPasswordAsync(user, command.Password))
        {
            await userManager.AccessFailedAsync(user);

            if (!await userManager.IsLockedOutAsync(user))
            {
                _logger.LogInformation("Invalid Authentication", DateTime.Now);

                return Result.Failure<AuthResponse>(UserErrors.Unauthorized());
            }

            var content =
                $"Your account is locked out. To reset the password click this link: {command.ClientUri}";
            var message = new Message(new[] { command.Email },
                "Locked out account information", content);

            await emailSender.SendEmailAsync(message, cancellationToken);

            _logger.LogException("The account is locked out", DateTime.Now);

            return Result.Failure<AuthResponse>(UserErrors.LockedOut(new Guid(user.Id)));
        }

        if (await userManager.GetTwoFactorEnabledAsync(user))
            return await GenerateOtpFor2StepVerification(user, cancellationToken);

        var token = await jwtHandler.GenerateToken(user);

        await userManager.ResetAccessFailedCountAsync(user);

        var response = new AuthResponse();
        response.Token = token;

        return Result.Success(response);
    }

    private async Task<Result<AuthResponse>> GenerateOtpFor2StepVerification(User user,
        CancellationToken cancellationToken)
    {
        var providers = await userManager.GetValidTwoFactorProvidersAsync(user);

        if (!providers.Contains("Email"))
        {
            _logger.LogException("Invalid 2-Step Verification Provider.", DateTime.Now);

            return Result.Failure<AuthResponse>(UserErrors.Invalid2Step(new Guid(user.Id)));
        }

        var token = await userManager.GenerateTwoFactorTokenAsync(user, "Email");
        var message = new Message(new[] { user.Email }, "Authentication token", token);

        await emailSender.SendEmailAsync(message, cancellationToken);

        var response = new AuthResponse();
        response.Is2StepVerificationRequired = true;
        response.Provider = "Email";

        return Result.Success(response);
    }
}
