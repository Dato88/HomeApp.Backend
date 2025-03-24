using Domain.Entities.User;

namespace Application.Cruds.Interfaces;

public interface IUserCrud
{
    Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken);

    // Task<(IdentityResult, User)> RegisterAsync(RegisterUserDto registerUserDto, CancellationToken cancellationToken);

    Task<User> GetUserAsync(string email, CancellationToken cancellationToken);
}
