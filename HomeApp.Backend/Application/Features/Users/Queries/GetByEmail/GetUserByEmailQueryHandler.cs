using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.Queries.GetByEmail;

internal sealed class GetUserByEmailQueryHandler(UserManager<User> userManager, IUserContext userContext)
    : IQueryHandler<GetUserByEmailQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken)
    {
        var user = await userManager.Users
            .Where(u => u.Email == query.Email).Select(u => new UserResponse
            {
                Id = new Guid(u.Id), FirstName = u.FirstName, LastName = u.LastName, Email = u.Email
            }).FirstOrDefaultAsync(cancellationToken);

        if (user is null) return Result.Failure<UserResponse>(UserErrors.NotFoundByEmail);

        if (user.Id != userContext.UserId) return Result.Failure<UserResponse>(UserErrors.Unauthorized());

        return user;
    }
}
