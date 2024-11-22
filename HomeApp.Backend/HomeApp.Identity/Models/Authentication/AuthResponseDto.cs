namespace HomeApp.Identity.Models.Authentication;

public class AuthResponseDto
{
    public bool IsAuthSuccessful { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Token { get; set; }
}