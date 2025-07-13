namespace Application.Features.Users.Queries.GetByEmail;

public sealed record UserResponse
{
    public Guid UserId { get; init; }
    public string Email { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
}
