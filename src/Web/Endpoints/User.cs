using Auth.Api.Application.Users.Commands.ConfirmEmail;
using Auth.Api.Application.Users.Commands.LoginUser;
using Auth.Api.Application.Users.Commands.RegisterUser;
using Auth.Api.Application.Users.Queries.GetUserById;
using Auth.Api.Application.Users.Queries.IsEmailConfirmed;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Web.Endpoints;

public class User : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(RegisterUser, "register")
            .MapPost(LoginUser, "login")
            .MapPost(ConfirmEmail, "confirm-email")
            .MapGet(IsEmailConfirmed, "is-email-confirmed/{userId}")
            .MapGet(GetUserById, "{userId}");

        // .MapIdentityApi<ApplicationUser>();
    }

    [ProducesResponseType(typeof(Guid), 201)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    public async Task<IResult> RegisterUser(ISender sender, RegisterUserCommand registerUserCommand)
    {
        var result = await sender.Send(registerUserCommand);
        return Results.Created("register", result);
    }

    [ProducesResponseType(typeof(LoginUserResponse), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 401)]
    public async Task<LoginUserResponse> LoginUser(ISender sender, LoginUserCommand loginUserCommand)
    {
        return await sender.Send(loginUserCommand);
    }

    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<bool> ConfirmEmail(ISender sender, ConfirmEmailCommand confirmEmailCommand)
    {
        return await sender.Send(confirmEmailCommand);
    }

    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<bool> IsEmailConfirmed(ISender sender, Guid userId)
    {
        var isEmailConfirmedCommand = new IsEmailConfirmedQuery { UserId = userId };
        return await sender.Send(isEmailConfirmedCommand);
    }

    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<GetUserByIdResponse> GetUserById(ISender sender, Guid userId)
    {
        var getUserByIdQuery = new GetUserByIdQuery() { Id = userId };
        return await sender.Send(getUserByIdQuery);
    }
}
