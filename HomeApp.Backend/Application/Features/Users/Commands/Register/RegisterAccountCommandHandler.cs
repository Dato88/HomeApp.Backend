using Application.Abstractions.Logging;
using Application.Abstractions.Messaging;
using Application.Email;
using Application.Features.People.Commands;
using Application.Models.Email;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Users.Commands.Register;

internal sealed class RegisterAccountCommandHandler(
    IEmailSender emailSender,
    IPersonCommands personCommands,
    UserManager<User> userManager,
    IAppLogger<RegisterAccountCommandHandler> logger)
    : ICommandHandler<RegisterAccountCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterAccountCommand command, CancellationToken cancellationToken)
    {
        User user = command;

        // Check if email is already used
        if (await userManager.Users.AnyAsync(u => u.Email == command.Email, cancellationToken))
            return Result.Failure<Guid>(UserErrors.EmailNotUnique);

        // Create identity user
        var identityResult = await userManager.CreateAsync(user, command.Password);
        if (!identityResult.Succeeded)
        {
            var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
            logger.LogWarning($"User registration failed for email {command.Email}. Reason(s): {errors}");

            return Result.Failure<Guid>(UserErrors.CreateFailedWithMessage(errors));
        }

        // Create corresponding Person entry
        var createPersonResult = await personCommands.CreatePersonAsync(user, cancellationToken);
        if (createPersonResult.IsFailure)
        {
            foreach (var error in createPersonResult.Errors)
                logger.LogWarning(
                    $"Creating Person entity for user {command.Email} failed: {error.Description} ({error.Code})");

            await userManager.DeleteAsync(user);

            return Result.Failure<Guid>(createPersonResult.Errors.ToArray());
        }

        // Generate email confirmation token
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var param = new Dictionary<string, string?> { { "token", token }, { "email", command.Email } };
        var callback = command.ClientUri is null
            ? string.Empty
            : QueryHelpers.AddQueryString(command.ClientUri, param);

        // Send confirmation email
        var message = new Message(new[] { command.Email }, "Email Confirmation token", callback);
        await emailSender.SendEmailAsync(message, cancellationToken);

        logger.LogInformation($"User registered successfully with email {command.Email}");

        return Result.Success(new Guid(user.Id));
    }
}
