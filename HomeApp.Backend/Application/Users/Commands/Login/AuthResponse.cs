namespace Application.Users.Commands.Login;

public sealed record AuthResponse
{
    public string? Token { get; set; }
    public bool Is2StepVerificationRequired { get; set; }
    public string? Provider { get; set; }
}
