using Auth.Api.Application.Common.Interfaces;
using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Resource;

namespace Auth.Api.Application.Users.Commands.EmailConfirmationToken;

public record GetEmailConfirmationTokenCommand : IRequest<GetEmailConfirmationTokenResponse>
{
}

public class
    GetEmailConfirmationTokenCommandHandler : IRequestHandler<GetEmailConfirmationTokenCommand,
        GetEmailConfirmationTokenResponse>
{
    private readonly IUser _user;
    private readonly IUserManagerService _userManagerService;

    public GetEmailConfirmationTokenCommandHandler(IUserManagerService userManagerService, IUser user)
    {
        _userManagerService = userManagerService;
        _user = user;
    }

    public async Task<GetEmailConfirmationTokenResponse> Handle(GetEmailConfirmationTokenCommand request,
        CancellationToken cancellationToken)
    {
        var token = await _userManagerService.GenerateEmailConfirmationTokenAsync(_user.Id!.Value);

        if (token is null)
        {
            throw new Exception(GeneralErrorMessages.UnkownError);
        }

        return new GetEmailConfirmationTokenResponse(token);
    }
}
