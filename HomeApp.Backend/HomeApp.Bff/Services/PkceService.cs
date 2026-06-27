using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace HomeApp.Bff.Services;

public sealed class PkceService
{
    public (string Verifier, string Challenge) GenerateChallenge()
    {
        var verifierBytes = RandomNumberGenerator.GetBytes(32);
        var verifier = Base64UrlEncode(verifierBytes);

        var challengeBytes = SHA256.HashData(Encoding.ASCII.GetBytes(verifier));
        var challenge = Base64UrlEncode(challengeBytes);

        return (verifier, challenge);
    }

    public string GenerateState()
    {
        var stateBytes = RandomNumberGenerator.GetBytes(16);
        return Base64UrlEncode(stateBytes);
    }

    private static string Base64UrlEncode(byte[] bytes) =>
        WebEncoders.Base64UrlEncode(bytes);
}
