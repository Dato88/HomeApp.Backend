using Application.Abstractions.Messaging;
using Application.DTOs.Authentication;
using Application.Email;
using Application.Models.Email;
using Domain.Entities.User;
using Infrastructure.Authorization.Handler;
using Infrastructure.Logger;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.Users.Commands.Login;

internal sealed class LoginUserCommandHandler(
    JwtHandler jwtHandler,
    UserManager<User> userManager,
    IEmailSender emailSender,
    ILogger<LoginUserCommandHandler> logger) : ICommandHandler<LoginUserCommand, AuthResponseDto>
{
    private readonly LoggerExtension<LoginUserCommandHandler> _logger = new(logger);

    public async Task<Result<AuthResponseDto>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var response = new AuthResponseDto();
        var user = await userManager.FindByEmailAsync(command.Email);

        if (!await userManager.IsEmailConfirmedAsync(user))
        {
            response.IsAuthSuccessful = false;
            response.ErrorMessage = "Email is not confirmed";

            _logger.LogException("Email is not confirmed", DateTime.Now);

            return response;
        }

        if (!await userManager.CheckPasswordAsync(user, command.Password))
        {
            await userManager.AccessFailedAsync(user);

            if (!await userManager.IsLockedOutAsync(user))
            {
                response.IsAuthSuccessful = false;
                response.ErrorMessage = "Invalid Authentication";

                _logger.LogException("Invalid Authentication", DateTime.Now);

                return response;
            }

            var content =
                $"Your account is locked out. To reset the password click this link: {command.ClientUri}";
            var message = new Message(new[] { command.Email },
                "Locked out account information", content);

            await emailSender.SendEmailAsync(message, cancellationToken);

            response.IsAuthSuccessful = false;
            response.ErrorMessage = "The account is locked out";

            _logger.LogException("The account is locked out", DateTime.Now);

            return response;
        }

        if (await userManager.GetTwoFactorEnabledAsync(user))
            return await GenerateOtpFor2StepVerification(user, cancellationToken);

        var token = await jwtHandler.GenerateToken(user);

        await userManager.ResetAccessFailedCountAsync(user);

        response.IsAuthSuccessful = true;
        response.Token = token;

        return response;
    }

    private async Task<AuthResponseDto> GenerateOtpFor2StepVerification(User user, CancellationToken cancellationToken)
    {
        var response = new AuthResponseDto();
        var providers = await userManager.GetValidTwoFactorProvidersAsync(user);

        if (!providers.Contains("Email"))
        {
            response.IsAuthSuccessful = false;
            response.ErrorMessage = "Invalid 2-Step Verification Provider.";

            _logger.LogException("Invalid 2-Step Verification Provider.", DateTime.Now);

            return response;
        }

        var token = await userManager.GenerateTwoFactorTokenAsync(user, "Email");
        var message = new Message(new[] { user.Email }, "Authentication token", token);

        await emailSender.SendEmailAsync(message, cancellationToken);

        response.Is2StepVerificationRequired = true;
        response.Provider = "Email";

        return response;
    }
}
