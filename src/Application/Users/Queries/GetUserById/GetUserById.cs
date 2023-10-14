using Auth.Api.Application.Common.Interfaces;

namespace Auth.Api.Application.Users.Queries.GetUserById;

public record GetUserByIdCommand : IRequest<GetUserByIdDto>
{
    public Guid Id { get; set; }
}

public class GetUserByIdCommandHandler : IRequestHandler<GetUserByIdCommand, GetUserByIdDto>
{
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public GetUserByIdCommandHandler(IIdentityService identityService, IMapper mapper)
    {
        _identityService = identityService;
        _mapper = mapper;
    }

    public async Task<GetUserByIdDto> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
    {
        string? userName = await _identityService.GetUserNameAsync(request.Id);

        Guard.Against.NotFound("user", userName);

        return new GetUserByIdDto { UserName = userName };
    }
}
