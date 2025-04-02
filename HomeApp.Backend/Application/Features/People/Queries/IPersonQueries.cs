using Application.Features.People.Dtos;

namespace Application.Features.People.Queries;

public interface IPersonQueries
{
    Task<PersonDto?> GetUserPersonAsync(CancellationToken cancellationToken);
    Task<PersonDto?> GetPersonByEmailAsync(string email, CancellationToken cancellationToken);
}
