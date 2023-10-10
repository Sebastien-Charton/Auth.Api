using Auth.Api.Application.Users.Commands.RegisterUser;
using Auth.Api.Infrastructure.Identity;

namespace Auth.Api.Web.Endpoints;

public class Users : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(RegisterUser)
            .MapIdentityApi<ApplicationUser>();
    }

    public async Task<Guid> RegisterUser(ISender sender, RegisterUserCommand registerUserCommand)
    {
        return await sender.Send(registerUserCommand);
    }
}
