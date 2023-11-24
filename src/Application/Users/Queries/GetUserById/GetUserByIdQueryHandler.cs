using Auth.Api.Application.Common.Interfaces.Identity.Services;

namespace Auth.Api.Application.Users.Queries.GetUserById;

public record GetUserByIdQuery : IRequest<GetUserByIdResponse>
{
    public Guid Id { get; set; }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly IUserManagerService _userManagerService;

    public GetUserByIdQueryHandler(IUserManagerService userManagerService, IMapper mapper)
    {
        _userManagerService = userManagerService;
        _mapper = mapper;
    }

    public async Task<GetUserByIdResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManagerService.GetUserByIdAsync(request.Id);

        Guard.Against.NotFound(nameof(user), user);

        return new GetUserByIdResponse(user.Id, user.Email!, user.UserName!);
    }
}
