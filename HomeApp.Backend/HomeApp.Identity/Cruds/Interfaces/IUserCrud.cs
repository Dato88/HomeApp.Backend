using HomeApp.Identity.Models;

namespace HomeApp.Identity.Cruds.Interfaces;

public interface IUserCrud
{
    Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken);

    Task<IdentityResult> RegisterAsync(RegisterUserDto registerUserDto, CancellationToken cancellationToken);
}