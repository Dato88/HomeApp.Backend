using SharedKernel.ValueObjects;

namespace Application.Abstractions.Authentication;

public interface IUserContext
{
    PersonId PersonId { get; }
    UserEmail UserEmail { get; }
    UserId UserId { get; }
}
