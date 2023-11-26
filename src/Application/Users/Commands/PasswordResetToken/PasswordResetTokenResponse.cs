namespace Auth.Api.Application.Users.Commands.PasswordResetToken;

public class PasswordResetTokenResponse
{
    public PasswordResetTokenResponse(string token)
    {
        Token = token;
    }

    public string Token { get; set; }
}
