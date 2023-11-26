using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Resource;

namespace Auth.Api.Application.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<bool>
{
    public DeleteUserCommand(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; set; }
}

public class
    DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand,
        bool>
{
    private readonly IUserManagerService _userManagerService;

    public DeleteUserCommandHandler(IUserManagerService userManagerService)
    {
        _userManagerService = userManagerService;
    }

    public async Task<bool> Handle(DeleteUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userManagerService.GetUserByIdAsync(request.UserId);

        Guard.Against.NotFound(nameof(user), user);

        var deleteUserResult = await _userManagerService.DeleteUserAsync(request.UserId);

        if (!deleteUserResult.Succeeded)
        {
            throw new Exception(GeneralErrorMessages.UnkownError);
        }

        return true;
    }
}
