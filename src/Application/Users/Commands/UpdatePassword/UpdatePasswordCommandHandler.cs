using Auth.Api.Application.Common.Interfaces;
using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Resource;

namespace Auth.Api.Application.Users.Commands.UpdatePassword;

public record UpdatePasswordCommand : IRequest<bool>
{
    public string NewPassword { get; set; } = null!;
    public string CurrentPassword { get; set; } = null!;
}

public class
    UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand,
        bool>
{
    private readonly IUser _user;
    private readonly IUserManagerService _userManagerService;

    public UpdatePasswordCommandHandler(IUserManagerService userManagerService, IUser user)
    {
        _userManagerService = userManagerService;
        _user = user;
    }

    public async Task<bool> Handle(UpdatePasswordCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userManagerService.GetUserByIdAsync(_user.Id!.Value);

        var changePasswordResult =
            await _userManagerService.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);

        if (!changePasswordResult.Succeeded)
        {
            throw new Exception(GeneralErrorMessages.UnkownError);
        }

        return true;
    }
}
