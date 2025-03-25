using Domain.Entities.User;

namespace Application.Abstractions.Authentication;

public interface ITokenProvider
{
    string Create(User user);
}
