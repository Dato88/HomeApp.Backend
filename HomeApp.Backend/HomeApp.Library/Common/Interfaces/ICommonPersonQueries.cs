using HomeApp.Library.People.Dtos;

namespace HomeApp.Library.Common.Interfaces;

public interface ICommonPersonQueries
{
    Task<PersonDto?> GetUserPersonAsync(CancellationToken cancellationToken);
    Task<PersonDto?> GetPersonByEmailAsync(string email, CancellationToken cancellationToken);
}
