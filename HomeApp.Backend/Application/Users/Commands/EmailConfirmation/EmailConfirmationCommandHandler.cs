using Application.Abstractions.Messaging;
using Domain.Entities.User;
using Infrastructure.Logger;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.Users.Commands.EmailConfirmation;

internal sealed class EmailConfirmationCommandHandler(
    UserManager<User> userManager,
    ILogger<EmailConfirmationCommandHandler> logger)
    : ICommandHandler<EmailConfirmationCommand, Guid>
{
    private readonly LoggerExtension<EmailConfirmationCommandHandler> _logger = new(logger);

    public async Task<Result<Guid>> Handle(EmailConfirmationCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);

        if (user == null)
        {
            _logger.LogWarning("Email is not found", DateTime.Now);

            return Result.Failure<Guid>(UserErrors.NotFoundByEmail);
        }

        var confirmResult = await userManager.ConfirmEmailAsync(user, command.Token);

        if (!confirmResult.Succeeded)
        {
            _logger.LogWarning("Invalid Email Confirmation Request", DateTime.Now);

            return Result.Failure<Guid>(UserErrors.Unauthorized());
        }

        return Result.Success(new Guid(user.Id));
    }
}
