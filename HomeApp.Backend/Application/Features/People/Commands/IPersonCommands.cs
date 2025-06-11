using Domain.Entities.People;
using SharedKernel;
using SharedKernel.ValueObjects;

namespace Application.Features.People.Commands;

public interface IPersonCommands
{
    Task<Result<PersonId>> CreatePersonAsync(Person person, CancellationToken cancellationToken);
    Task<Result> UpdatePersonAsync(Person person, CancellationToken cancellationToken);
    Task<Result> DeletePersonAsync(PersonId personId, CancellationToken cancellationToken);
}
