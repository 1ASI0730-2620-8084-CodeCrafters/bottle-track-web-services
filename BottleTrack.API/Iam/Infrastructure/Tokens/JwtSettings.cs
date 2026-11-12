namespace BottleTrack.API.Iam.Infrastructure.Tokens;

public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;

    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public int ExpiresInDays { get; set; }
}
