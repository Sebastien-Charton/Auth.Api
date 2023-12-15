namespace Auth.Api.Application.Users.Commands.LoginUser;

public class LoginUserResponse
{
    public required string Token { get; set; }
    public required Guid UserId { get; set; }
}
