using Domain.Entities.User;

namespace Application.Cruds.Interfaces;

public interface IUserCrud
{
    Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken);
}
