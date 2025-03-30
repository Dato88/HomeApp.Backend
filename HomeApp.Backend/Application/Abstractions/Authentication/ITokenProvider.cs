using Domain.Entities.User;

namespace Application.Abstractions.Authentication;

public interface ITokenProvider
{
    Task<string> Create(User user);
}
