using Auth.Api.Application.Common.Interfaces.ServiceAgents;
using Auth.Api.Infrastructure.Options;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Auth.Api.Infrastructure.ServiceAgents;

public class MailServiceAgent : IMailServiceAgent
{
    private readonly IOptions<MailOptions> _mailOptions;
    private readonly ISendGridClient _sendGridClient;

    public MailServiceAgent(ISendGridClient sendGridClient, IOptions<MailOptions> mailOptions)
    {
        _sendGridClient = sendGridClient;
        _mailOptions = mailOptions;
    }

    public async Task<bool> SendMail(string toEmail, string toName, string subject, string plainTextContent,
        string htmlContent)
    {
        // TODO retry mechanism and how to handle unsuccessful status code
        var from = new EmailAddress(_mailOptions.Value.FromEmail, _mailOptions.Value.FromName);
        var to = new EmailAddress(toEmail, toName);

        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

        var response = await _sendGridClient.SendEmailAsync(msg);

        return response.IsSuccessStatusCode;
    }
}
