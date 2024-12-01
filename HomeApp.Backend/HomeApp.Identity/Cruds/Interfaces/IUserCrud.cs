using HomeApp.Identity.Entities.DataTransferObjects.Register;
using HomeApp.Identity.Entities.Models;

namespace HomeApp.Identity.Cruds.Interfaces;

public interface IUserCrud
{
    Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken);

    Task<IdentityResult> RegisterAsync(RegisterUserDto registerUserDto, CancellationToken cancellationToken);
}