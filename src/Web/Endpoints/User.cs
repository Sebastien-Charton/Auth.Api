using Auth.Api.Application.Users.Commands.ConfirmEmail;
using Auth.Api.Application.Users.Commands.EmailConfirmationToken;
using Auth.Api.Application.Users.Commands.LoginUser;
using Auth.Api.Application.Users.Commands.PasswordResetToken;
using Auth.Api.Application.Users.Commands.RegisterUser;
using Auth.Api.Application.Users.Commands.RegisterUserAdmin;
using Auth.Api.Application.Users.Queries.GetUserById;
using Auth.Api.Application.Users.Queries.IsEmailConfirmed;
using Auth.Api.Application.Users.Queries.IsEmailExists;
using Auth.Api.Application.Users.Queries.IsUserNameExists;
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
            .MapPost(RegisterUserAdmin, "register-admin")
            .MapPost(LoginUser, "login")
            .MapPost(ConfirmEmail, "confirm-email")
            .MapGet(IsEmailConfirmed, "is-email-confirmed")
            .MapPost(GetEmailConfirmationToken, "confirmation-email-token")
            .MapPost(GetEmailResetToken, "password-reset-token")
            .MapGet(GetUserById, "{userId}")
            .MapGet(IsEmailExists, "is-email-exists/{email}")
            .MapGet(IsUserNameExists, "is-username-exists/{userName}");
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

    [Authorize(Policy = Policies.IsAdministrator)]
    [ProducesResponseType(typeof(Guid), 201)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [EndpointDescription("Create a new user with specific roles")]
    public async Task<IResult> RegisterUserAdmin(ISender sender, RegisterUserAdminCommand registerUserAdmanCommand)
    {
        var result = await sender.Send(registerUserAdmanCommand);
        return Results.Created("register-admin", result);
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
    public async Task<bool> IsEmailConfirmed(ISender sender)
    {
        var isEmailConfirmedCommand = new IsEmailConfirmedQuery();
        return await sender.Send(isEmailConfirmedCommand);
    }

    [Authorize(Policy = Policies.IsAdministrator)]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    [EndpointDescription("Get a user by user Id")]
    public async Task<GetUserByIdResponse> GetUserById(ISender sender, Guid userId)
    {
        var getUserByIdQuery = new GetUserByIdQuery { Id = userId };
        return await sender.Send(getUserByIdQuery);
    }

    [Authorize(Policy = Policies.IsAdministrator)]
    [ProducesResponseType(typeof(GetEmailConfirmationTokenResponse), 200)]
    [EndpointDescription("Get a confirmation email token")]
    public async Task<GetEmailConfirmationTokenResponse> GetEmailConfirmationToken(ISender sender)
    {
        var getEmailConfirmationTokenCommand = new GetEmailConfirmationTokenCommand();
        return await sender.Send(getEmailConfirmationTokenCommand);
    }

    [AllowAnonymous]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [EndpointDescription("Return if a user already exists with the same email address")]
    public async Task<bool> IsEmailExists(ISender sender, string email)
    {
        return await sender.Send(new IsEmailExistsQuery { Email = email });
    }

    [AllowAnonymous]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [EndpointDescription("Return if a user already exists with the same username address")]
    public async Task<bool> IsUserNameExists(ISender sender, string userName)
    {
        return await sender.Send(new IsUserNameExistsQuery { UserName = userName });
    }

    [Authorize(Policy = Policies.AllUsers)]
    [ProducesResponseType(typeof(PasswordResetTokenResponse), 200)]
    [EndpointDescription("Return a reset password token")]
    public async Task<PasswordResetTokenResponse> GetEmailResetToken(ISender sender)
    {
        return await sender.Send(new PasswordResetTokenCommand());
    }
}
