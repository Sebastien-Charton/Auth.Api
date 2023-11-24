namespace Auth.Api.Infrastructure.Options;

public class SendGridOptions
{
    public required string ApiKey { get; set; }
    public required string FromEmail { get; set; }
    public required string FromName { get; set; }
}
