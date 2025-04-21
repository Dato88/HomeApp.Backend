using Application.Abstractions.Messaging;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Application.Features.Users.Queries.GetAllUser;

internal sealed class GetAllUserQueryHandler(UserManager<User> userManager)
    : IQueryHandler<GetAllUserQuery, IEnumerable<User>>
{
    public async Task<Result<IEnumerable<User>>> Handle(GetAllUserQuery query, CancellationToken cancellationToken)
    {
        await Task.Delay(0);

        var user = userManager.Users.AsEnumerable();

        if (!user.Any()) return Result.Failure<IEnumerable<User>>(UserErrors.NotFoundAll);

        return Result.Success(user);
    }
}
