namespace HomeApp.Identity.Cruds.Interfaces;

public interface IUserCrud
{
    Task<IdentityResult> RegisterAsync(RegisterUserDto registerUserDto, CancellationToken cancellationToken);
}