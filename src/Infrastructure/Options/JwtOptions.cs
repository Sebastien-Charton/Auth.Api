namespace Auth.Api.Infrastructure.Options;

public class JwtOptions
{
    public string SecurityKey { get; set; } = null!;

    public string Issuer { get; set; } = null!;

    public string Audience { get; set; } = null!;

    public int ExpiryInDays { get; set; }
}
