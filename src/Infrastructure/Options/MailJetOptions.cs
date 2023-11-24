namespace Auth.Api.Infrastructure.Options;

public class MailJetOptions
{
    public required string ApiKey { get; set; }
    public required string ApiSecret { get; set; }
    public required string FromEmail { get; set; }
    public required string FromName { get; set; }
}
