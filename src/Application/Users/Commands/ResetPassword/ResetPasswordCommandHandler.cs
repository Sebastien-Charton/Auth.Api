using Auth.Api.Application.Common.Interfaces;
using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Resource;

namespace Auth.Api.Application.Users.Commands.ResetPassword;

public record ResetPasswordCommand : IRequest<bool>
{
    public string NewPassword { get; set; } = null!;
    public string Token { get; set; } = null!;
}

public class
    ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand,
        bool>
{
    private readonly IUser _user;
    private readonly IUserManagerService _userManagerService;

    public ResetPasswordCommandHandler(IUserManagerService userManagerService, IUser user)
    {
        _userManagerService = userManagerService;
        _user = user;
    }

    public async Task<bool> Handle(ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userManagerService.GetUserByIdAsync(_user.Id!.Value);

        var resetPasswordResult =
            await _userManagerService.ResetPasswordAsync(user!, request.Token, request.NewPassword);

        if (!resetPasswordResult.Succeeded)
        {
            throw new Exception(GeneralErrorMessages.UnkownError);
        }

        return true;
    }
}
