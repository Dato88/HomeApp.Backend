using Domain.ValueObjects;

namespace Application.Abstractions.Authentication;

public interface IUserContext
{
    int PersonId { get; }
    UserEmail UserEmail { get; }
    Guid UserId { get; }
}
