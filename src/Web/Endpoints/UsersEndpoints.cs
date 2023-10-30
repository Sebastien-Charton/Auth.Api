using Auth.Api.Application.Users.Commands.ConfirmEmail;
using Auth.Api.Application.Users.Commands.IsEmailConfirmed;
using Auth.Api.Application.Users.Commands.LoginUser;
using Auth.Api.Application.Users.Commands.RegisterUser;

namespace Auth.Api.Web.Endpoints;

public class UsersEndpoints : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(RegisterUser, "register")
            .MapPost(LoginUser, "login")
            .MapPost(ConfirmEmail, "confirm-email")
            .MapGet(IsEmailConfirmed, "is-email-confirmed/{userId}");
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

    public async Task<bool> ConfirmEmail(ISender sender, ConfirmEmailCommand confirmEmailCommand)
    {
        return await sender.Send(confirmEmailCommand);
    }

    public async Task<bool> IsEmailConfirmed(ISender sender, Guid userId)
    {
        var isEmailConfirmedCommand = new IsEmailConfirmedCommand { UserId = userId };
        return await sender.Send(isEmailConfirmedCommand);
    }
}
