﻿namespace Auth.Api.Infrastructure.Options;

public class MailJetOptions
{
    public string ApiKey { get; set; } = null!;
    public string ApiSecret { get; set; } = null!;
    public string FromEmail { get; set; } = null!;
    public string FromName { get; set; } = null!;
}
