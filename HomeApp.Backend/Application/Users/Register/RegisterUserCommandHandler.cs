using Application.Common.Interfaces.People;
using Application.DTOs.Register;
using Application.Email;
using Application.Models.Email;
using Domain.Entities.User;
using Infrastructure.Logger;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Application.Users.Register;

internal sealed class RegisterUserCommandHandler(
    IEmailSender emailSender,
    ICommonPersonCommands commonPersonCommands,
    UserManager<User> userManager,
    ILogger<RegisterUserCommandHandler> logger)
    : IRequestHandler<RegisterUserCommand, RegistrationResponseDto>
{
    private readonly LoggerExtension<RegisterUserCommandHandler> _logger = new(logger);

    public async Task<RegistrationResponseDto> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var response = new RegistrationResponseDto();
        User user = command;

        var result = await userManager.CreateAsync(user, command.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);

            response.IsSuccessfulRegistration = false;
            response.Errors = errors;

            _logger.LogException("Email is not confirmed", DateTime.Now);

            return response;
        }

        await commonPersonCommands.CreatePersonAsync(user, cancellationToken);

        var token = await userManager.GenerateEmailConfirmationTokenAsync(command);
        var param = new Dictionary<string, string?> { { "token", token }, { "email", command.Email } };
        var callback = command.ClientUri is null
            ? string.Empty
            : QueryHelpers.AddQueryString(command.ClientUri, param);
        var message = new Message(new[] { command.Email }, "Email Confirmation token", callback);

        await emailSender.SendEmailAsync(message, cancellationToken);

        response.IsSuccessfulRegistration = true;

        return response;
    }
}
