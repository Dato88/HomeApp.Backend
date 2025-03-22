using Application.People.Dtos;

namespace Application.Common.Interfaces.People;

public interface ICommonPersonQueries
{
    Task<PersonDto?> GetUserPersonAsync(CancellationToken cancellationToken);
    Task<PersonDto?> GetPersonByEmailAsync(string email, CancellationToken cancellationToken);
}
