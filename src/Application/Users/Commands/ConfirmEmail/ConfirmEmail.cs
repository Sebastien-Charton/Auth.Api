using Auth.Api.Application.Common.Interfaces.Identity.Services;
using FluentValidation.Results;

namespace Auth.Api.Application.Users.Commands.ConfirmEmail;

public record ConfirmEmailCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = null!;
}

public class ValidateEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, bool>
{
    private readonly IUserManagerService _userManagerService;

    public ValidateEmailCommandHandler(IUserManagerService userManagerService)
    {
        _userManagerService = userManagerService;
    }

    public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var result = await _userManagerService.ConfirmEmailAsync(request.UserId, request.Token);

        if (!result.Succeeded)
        {
            throw new ValidationException(result.Errors.Select(x =>
                new ValidationFailure(nameof(ConfirmEmailCommand), x)));
        }

        return true;
    }
}
