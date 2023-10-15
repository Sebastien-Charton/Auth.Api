using Auth.Api.Application.Users.Commands.LoginUser;
using Auth.Api.Application.Users.Commands.RegisterUser;

namespace Auth.Api.Web.Endpoints;

public class Users : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(RegisterUser, "register")
            .MapPost(LoginUser, "login");
        // .MapIdentityApi<ApplicationUser>();
    }

    public async Task<Guid> RegisterUser(ISender sender, RegisterUserCommand registerUserCommand)
    {
        return await sender.Send(registerUserCommand);
    }

    public async Task<LoginUserResponse> LoginUser(ISender sender, LoginUserCommand loginUserCommand)
    {
        return await sender.Send(loginUserCommand);
    }
}
