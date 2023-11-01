using Auth.Api.Application.Common.Interfaces;
using DotLiquid;
using Resource;

namespace Auth.Api.Infrastructure.Services.HtmlGeneration;

public class DotLiquidHtmlGenerator : IHtmlGenerator
{
    public string GenerateConfirmationEmail(string userName, string token)
    {
        var htmlTemplate = HtmlTemplates.EmailConfirmationTemplate;
        var dictionary = new Dictionary<string, object> { { "userName", userName }, { "token", token } };
        var template = Template.Parse(htmlTemplate);
        return template.Render(Hash.FromDictionary(dictionary));
    }
}
