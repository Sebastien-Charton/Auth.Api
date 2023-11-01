using Auth.Api.Application.Common.Interfaces.ServiceAgents;
using Auth.Api.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Auth.Api.Infrastructure.ServiceAgents;

public class SendGridServiceAgent : IMailServiceAgent
{
    private readonly ILogger<SendGridServiceAgent> _logger;
    private readonly IOptions<MailOptions> _mailOptions;
    private readonly ISendGridClient _sendGridClient;

    public SendGridServiceAgent(ISendGridClient sendGridClient, IOptions<MailOptions> mailOptions,
        ILogger<SendGridServiceAgent> logger)
    {
        _sendGridClient = sendGridClient;
        _mailOptions = mailOptions;
        _logger = logger;
    }

    public async Task<bool> SendMail(string toEmail, string toName, string subject, string plainTextContent,
        string htmlContent)
    {
        var from = new EmailAddress(_mailOptions.Value.FromEmail, _mailOptions.Value.FromName);
        var to = new EmailAddress(toEmail, toName);

        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

        var response = await _sendGridClient.SendEmailAsync(msg);

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
