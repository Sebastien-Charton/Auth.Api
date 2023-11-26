using Auth.Api.Application.Common.Interfaces;
using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Resource;

namespace Auth.Api.Application.Users.Commands.PasswordResetToken;

public class PasswordResetTokenCommand : IRequest<PasswordResetTokenResponse>
{
}

public class PasswordResetTokenCommandHandler : IRequestHandler<PasswordResetTokenCommand,
    PasswordResetTokenResponse>
{
    private readonly IMapper _mapper;
    private readonly IUser _user;
    private readonly IUserManagerService _userManagerService;

    public PasswordResetTokenCommandHandler(IUserManagerService userManagerService, IMapper mapper, IUser user)
    {
        _userManagerService = userManagerService;
        _mapper = mapper;
        _user = user;
    }

    public async Task<PasswordResetTokenResponse> Handle(PasswordResetTokenCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userManagerService.GetUserByIdAsync(_user.Id!.Value);
        var token = await _userManagerService.GenerateResetPasswordTokenAsync(user!);

        if (token is null)
        {
            throw new Exception(GeneralErrorMessages.UnkownError);
        }

        return new PasswordResetTokenResponse(token);
    }
}
