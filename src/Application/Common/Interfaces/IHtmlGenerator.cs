namespace Auth.Api.Application.Common.Interfaces;

public interface IHtmlGenerator
{
    string GenerateConfirmationEmail(string userName, string token);
}
