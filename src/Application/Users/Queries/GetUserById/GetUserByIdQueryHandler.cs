using Auth.Api.Application.Common.Interfaces.Identity.Services;

namespace Auth.Api.Application.Users.Queries.GetUserById;

public record GetUserByIdQuery : IRequest<GetUserByIdResponse>
{
    public Guid Id { get; set; }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdResponse>
{
    private readonly IUserManagerService _userManagerService;

    public GetUserByIdQueryHandler(IUserManagerService userManagerService)
    {
        _userManagerService = userManagerService;
    }

    public async Task<GetUserByIdResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManagerService.GetUserByIdAsync(request.Id);

        Guard.Against.NotFound(nameof(user), user);

        return new GetUserByIdResponse(user.Id, user.Email!, user.UserName!);
    }
}
