namespace Auth.Api.Application.Common.Interfaces.ServiceAgents;

public interface IMailServiceAgent
{
    Task<bool> SendMail(string toEmail, string toName, string subject, string plainTextContent, string htmlContent);
}
