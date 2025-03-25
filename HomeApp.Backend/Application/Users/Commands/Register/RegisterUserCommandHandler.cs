using Application.Abstractions.Logging;
using Application.Abstractions.Messaging;
using Application.Common.Interfaces.People;
using Application.Email;
using Application.Models.Email;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.Commands.Register;

internal sealed class RegisterUserCommandHandler(
    IEmailSender emailSender,
    ICommonPersonCommands commonPersonCommands,
    UserManager<User> userManager,
    IAppLogger<RegisterUserCommandHandler> logger)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        User user = command;

        if (await userManager.Users.AnyAsync(u => u.Email == command.Email, cancellationToken))
            return Result.Failure<Guid>(UserErrors.EmailNotUnique);

        var result = await userManager.CreateAsync(user, command.Password);

        if (!result.Succeeded)
        {
            logger.LogException("Email is not confirmed");

            return Result.Failure<Guid>(UserErrors.EmailNotUnique);
        }

        await commonPersonCommands.CreatePersonAsync(user, cancellationToken);

        var token = await userManager.GenerateEmailConfirmationTokenAsync(command);
        var param = new Dictionary<string, string?> { { "token", token }, { "email", command.Email } };
        var callback = command.ClientUri is null
            ? string.Empty
            : QueryHelpers.AddQueryString(command.ClientUri, param);
        var message = new Message(new[] { command.Email }, "Email Confirmation token", callback);

        await emailSender.SendEmailAsync(message, cancellationToken);

        return Result.Success(new Guid(user.Id));
    }
}
