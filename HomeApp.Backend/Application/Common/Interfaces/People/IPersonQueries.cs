using Application.Features.People.Dtos;

namespace Application.Common.Interfaces.People;

public interface IPersonQueries
{
    Task<PersonDto?> GetUserPersonAsync(CancellationToken cancellationToken);
    Task<PersonDto?> GetPersonByEmailAsync(string email, CancellationToken cancellationToken);
}
