using Auth.Api.Application.Common.Exceptions;
using Auth.Api.Application.Common.Interfaces;
using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Application.Common.Interfaces.ServiceAgents;
using Auth.Api.Application.Common.Options;
using Microsoft.Extensions.Options;
using Resource;

namespace Auth.Api.Application.Users.Commands.SendPasswordResetToken;

public class SendPasswordResetTokenCommand : IRequest<bool>
{
}

public class
    SendPasswordResetTokenCommandHandler : IRequestHandler<SendPasswordResetTokenCommand,
        bool>
{
    private readonly IOptions<FeatureOptions> _featureOptions;
    private readonly IHtmlGenerator _htmlGenerator;
    private readonly IMailServiceAgent _mailServiceAgent;
    private readonly IUser _user;
    private readonly IUserManagerService _userManagerService;

    public SendPasswordResetTokenCommandHandler(IUserManagerService userManagerService, IUser user,
        IOptions<FeatureOptions> featureOptions, IHtmlGenerator htmlGenerator, IMailServiceAgent mailServiceAgent)
    {
        _userManagerService = userManagerService;
        _user = user;
        _featureOptions = featureOptions;
        _htmlGenerator = htmlGenerator;
        _mailServiceAgent = mailServiceAgent;
    }

    public async Task<bool> Handle(SendPasswordResetTokenCommand request,
        CancellationToken cancellationToken)
    {
        if (_featureOptions.Value.ActivateSendingOfConfirmationEmails)
        {
            var user = await _userManagerService.GetUserByIdAsync(_user.Id!.Value);

            var token = await _userManagerService.GenerateResetPasswordTokenAsync(user!);

            var htmlContent = _htmlGenerator.GenerateConfirmationEmail(_user.Email!, token!);

            await _mailServiceAgent.SendMail(_user.Email!, _user.UserName!,
                HtmlTemplates.EmailConfirmationTemplateTitle, "", htmlContent);

            return true;
        }

        throw new ForbiddenAccessException();
    }
}
