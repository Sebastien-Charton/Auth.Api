using Auth.Api.Application.Common.Interfaces.Identity.Services;

namespace Auth.Api.Application.Users.Queries.GetUserById;

public record GetUserByIdQuery : IRequest<GetUserByIdDto>
{
    public Guid Id { get; set; }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdDto>
{
    private readonly IMapper _mapper;
    private readonly IUserManagerService _userManagerService;

    public GetUserByIdQueryHandler(IUserManagerService userManagerService, IMapper mapper)
    {
        _userManagerService = userManagerService;
        _mapper = mapper;
    }

    public async Task<GetUserByIdDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        string? userName = await _userManagerService.GetUserNameAsync(request.Id);

        Guard.Against.NotFound("user", userName);

        return new GetUserByIdDto { UserName = userName };
    }
}
