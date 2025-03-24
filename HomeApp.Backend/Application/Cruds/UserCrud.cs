using Application.Cruds.Interfaces;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace Application.Cruds;

public class UserCrud(UserManager<User> userManager) : IUserCrud
{
    public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(0, cancellationToken);

        return userManager.Users;
    }
}
