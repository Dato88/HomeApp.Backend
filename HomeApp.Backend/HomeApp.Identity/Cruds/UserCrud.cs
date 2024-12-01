using HomeApp.Identity.Cruds.Interfaces;
using HomeApp.Identity.Entities.DataTransferObjects.Register;
using HomeApp.Identity.Entities.Models;

namespace HomeApp.Identity.Cruds;

public partial class UserCrud(UserManager<User> userManager) : IUserCrud
{
    public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(0, cancellationToken);

        return userManager.Users;
    }

    public async Task<IdentityResult> RegisterAsync(RegisterUserDto registerUserDto,
        CancellationToken cancellationToken)
    {
        await Task.Delay(0, cancellationToken);

        User user = registerUserDto;

        return await userManager.CreateAsync(user, registerUserDto.Password);
    }
}