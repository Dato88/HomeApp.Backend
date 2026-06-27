namespace Infrastructure.Configurations;

public sealed class DevUserContextOptions
{
    public int PersonId { get; set; }

    public Guid UserId { get; set; }

    public string Email { get; set; } = default!;
}
