using HomeApp.Identity.Cruds.Interfaces;
using HomeApp.Identity.Models;

namespace HomeApp.Identity.Cruds;

public partial class UserCrud(UserManager<User> userManager) : IUserCrud
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<IdentityResult> RegisterAsync(RegisterUserDto registerUserDto,
        CancellationToken cancellationToken)
    {
        User user = registerUserDto;

        return await _userManager.CreateAsync(user, registerUserDto.Password);
    }
}