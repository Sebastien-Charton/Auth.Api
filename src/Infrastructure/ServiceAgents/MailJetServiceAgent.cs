using Auth.Api.Application.Common.Interfaces.ServiceAgents;
using Auth.Api.Infrastructure.Options;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Auth.Api.Infrastructure.ServiceAgents;

public class MailJetServiceAgent : IMailServiceAgent
{
    private readonly ILogger<MailJetServiceAgent> _logger;
    private readonly IMailjetClient _mailjetClient;
    private readonly IOptions<MailJetOptions> _mailJetOptions;

    public MailJetServiceAgent(ILogger<MailJetServiceAgent> logger, IOptions<MailJetOptions> mailJetOptions,
        IMailjetClient mailjetClient)
    {
        _logger = logger;
        _mailJetOptions = mailJetOptions;
        _mailjetClient = mailjetClient;
    }

    public async Task<bool> SendMail(string toEmail, string toName, string subject, string plainTextContent,
        string htmlContent)
    {
        MailjetRequest a = new() { Resource = Send.Resource };

        a.Property(Send.FromEmail, _mailJetOptions.Value.FromEmail);
        a.Property(Send.FromName, _mailJetOptions.Value.FromName);
        a.Property(Send.To, toEmail);
        a.Property(Send.Subject, subject);
        a.Property(Send.HtmlPart, htmlContent);
        a.Property(Send.TextPart, plainTextContent);

        MailjetResponse response = await _mailjetClient.PostAsync(a);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Email sent successfully to {toEmail}", toEmail);
        }
        else
        {
            _logger.LogError("Error during the email sending to {toEmail} with the status code {statusCode}", toEmail,
                response.StatusCode);
        }

        return response.IsSuccessStatusCode;
    }
}
