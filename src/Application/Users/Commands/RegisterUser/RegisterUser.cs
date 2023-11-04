using Auth.Api.Application.Common.Interfaces;
using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Application.Common.Interfaces.ServiceAgents;
using Auth.Api.Domain.Constants;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Resource;
using ValidationException = Auth.Api.Application.Common.Exceptions.ValidationException;

namespace Auth.Api.Application.Users.Commands.RegisterUser;

public record RegisterUserCommand : IRequest<Guid>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IHtmlGenerator _htmlGenerator;
    private readonly ILogger<RegisterUserCommandHandler> _logger;
    private readonly IMailServiceAgent _mailServiceAgent;
    private readonly IUserManagerService _userManagerService;

    public RegisterUserCommandHandler(IUserManagerService userManagerService, IMailServiceAgent mailServiceAgent,
        IHtmlGenerator htmlGenerator, ILogger<RegisterUserCommandHandler> logger)
    {
        _userManagerService = userManagerService;
        _mailServiceAgent = mailServiceAgent;
        _htmlGenerator = htmlGenerator;
        _logger = logger;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUserWithUserName = await _userManagerService.IsUserNameExists(request.UserName);

        var validationExceptions = new List<ValidationFailure>();
        if (existingUserWithUserName)
        {
            var validationException =
                new ValidationFailure(nameof(request.UserName), UserErrorMessages.UserNameAlreadyExists);
            validationExceptions.Add(validationException);
        }

        var existingUserWithEmail = await _userManagerService.IsEmailExists(request.Email);

        if (existingUserWithEmail)
        {
            var validationException =
                new ValidationFailure(nameof(request.Email), UserErrorMessages.EmailAlreadyExists);
            validationExceptions.Add(validationException);
        }

        if (validationExceptions.Any())
        {
            throw new ValidationException(validationExceptions);
        }

        (Common.Models.Result Result, Guid userId) result = await _userManagerService.CreateUserAsync(request.UserName,
            request.Password,
            request.Email, request.PhoneNumber);

        if (result.Result.Errors.Any())
        {
            throw new ValidationException(
                result.Result.Errors.Select(x => new ValidationFailure(nameof(RegisterUserCommand), "error")));
        }

        await _userManagerService.AddToRolesAsync(result.userId, new[] { Roles.User });

        var token = await _userManagerService.GenerateEmailConfirmationToken(result.userId);

        var htmlContent = _htmlGenerator.GenerateConfirmationEmail(request.UserName, token!);

        await _mailServiceAgent.SendMail(request.Email, request.UserName,
            HtmlTemplates.EmailConfirmationTemplateTitle, "", htmlContent);

        return result.userId;
    }
}
