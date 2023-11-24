using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Resource;

namespace Auth.Api.Application.Users.Commands.EmailConfirmationToken;

public record GetEmailConfirmationTokenCommand : IRequest<GetEmailConfirmationTokenResponse>
{
    public Guid UserId { get; set; }
}

public class
    GetEmailConfirmationTokenCommandHandler : IRequestHandler<GetEmailConfirmationTokenCommand,
        GetEmailConfirmationTokenResponse>
{
    private readonly IMapper _mapper;
    private readonly IUserManagerService _userManagerService;

    public GetEmailConfirmationTokenCommandHandler(IUserManagerService userManagerService, IMapper mapper)
    {
        _userManagerService = userManagerService;
        _mapper = mapper;
    }

    public async Task<GetEmailConfirmationTokenResponse> Handle(GetEmailConfirmationTokenCommand request,
        CancellationToken cancellationToken)
    {
        var token = await _userManagerService.GenerateEmailConfirmationTokenAsync(request.UserId);

        if (token is null)
        {
            throw new Exception(GeneralErrorMessages.UnkownError);
        }

        return new GetEmailConfirmationTokenResponse(token);
    }
}
