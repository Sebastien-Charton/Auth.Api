using Auth.Api.Application.Users.Commands.ConfirmEmail;
using Auth.Api.Application.Users.Commands.EmailConfirmationToken;
using Auth.Api.Application.Users.Commands.LoginUser;
using Auth.Api.Application.Users.Commands.RegisterUser;
using Auth.Api.Application.Users.Queries.GetUserById;
using Auth.Api.Application.Users.Queries.IsEmailConfirmed;
using Auth.Api.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
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
            .MapPost(GetEmailConfirmationToken, "confirmation-email-token/{userId}")
            .MapGet(GetUserById, "{userId}");

        // .MapIdentityApi<ApplicationUser>();
    }

    [AllowAnonymous]
    [ProducesResponseType(typeof(Guid), 201)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [EndpointDescription("Create a new user")]
    public async Task<IResult> RegisterUser(ISender sender, RegisterUserCommand registerUserCommand)
    {
        var result = await sender.Send(registerUserCommand);
        return Results.Created("register", result);
    }

    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginUserResponse), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 401)]
    [EndpointDescription("Login an existing user")]
    public async Task<LoginUserResponse> LoginUser(ISender sender, LoginUserCommand loginUserCommand)
    {
        return await sender.Send(loginUserCommand);
    }

    [Authorize(Policy = Policies.AllUsers)]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    [EndpointDescription("Confirm email for a user")]
    public async Task<bool> ConfirmEmail(ISender sender, ConfirmEmailCommand confirmEmailCommand)
    {
        return await sender.Send(confirmEmailCommand);
    }

    [Authorize(Policy = Policies.AllUsers)]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    [EndpointDescription("Return if the email is registered or not")]
    public async Task<bool> IsEmailConfirmed(ISender sender, Guid userId)
    {
        var isEmailConfirmedCommand = new IsEmailConfirmedQuery { UserId = userId };
        return await sender.Send(isEmailConfirmedCommand);
    }

    [Authorize(Policy = Policies.IsAdministrator)]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    [EndpointDescription("Get a user by user Id")]
    public async Task<GetUserByIdResponse> GetUserById(ISender sender, Guid userId)
    {
        var getUserByIdQuery = new GetUserByIdQuery() { Id = userId };
        return await sender.Send(getUserByIdQuery);
    }

    [Authorize(Policy = Policies.IsAdministrator)]
    [ProducesResponseType(typeof(GetEmailConfirmationTokenResponse), 200)]
    [EndpointDescription("Get a confirmation email token")]
    public async Task<GetEmailConfirmationTokenResponse> GetEmailConfirmationToken(ISender sender, Guid userId)
    {
        var getEmailConfirmationTokenCommand = new GetEmailConfirmationTokenCommand { UserId = userId };
        return await sender.Send(getEmailConfirmationTokenCommand);
    }
}
