using Auth.Api.Application.Common.Interfaces;
using Auth.Api.Application.Common.Models;
using Auth.Api.Application.Users.Commands.RegisterUser;
using Auth.Api.Application.Users.Queries.GetUserById;

namespace Microsoft.Extensions.DependencyInjection.Users.Queries.GetUserById;

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
        var user = await _identityService.GetUserNameAsync(request.Id);

        Guard.Against.Null(user);
        
        return _mapper.Map<GetUserByIdDto>(user);
    }
}
