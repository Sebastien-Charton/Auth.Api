using Auth.Api.Application.Common.Exceptions;
using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Resource;

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
        var user = await _userManagerService.GetUserByIdAsync(request.UserId);

        Guard.Against.NotFound(nameof(user), user);

        var result = await _userManagerService.ConfirmEmailAsync(user, request.Token);

        if (result.Succeeded)
            return true;

        if (result.Errors.Any(x => x.Code == nameof(UserErrorMessages.InvalidToken)))
        {
            throw new BadRequestException(UserErrorMessages.InvalidToken);
        }

        throw new Exception(GeneralErrorMessages.UnkownError);
    }
}
