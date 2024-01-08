namespace Auth.Api.Application.Common.Options;

public class RefreshTokenOptions
{
    public required string SecurityKey { get; set; }
    public required int ExpiryInDays { get; set; }
}
