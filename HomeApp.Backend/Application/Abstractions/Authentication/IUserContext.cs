namespace Application.Abstractions.Authentication;

public interface IUserContext
{
    int PersonId { get; }
    string UserEmail { get; }
    Guid UserId { get; }
}
