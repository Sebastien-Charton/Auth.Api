namespace Auth.Api.Application.Users.Commands.EmailConfirmationToken;

public class GetEmailConfirmationTokenResponse
{
    public GetEmailConfirmationTokenResponse(string token)
    {
        Token = token;
    }

    public string Token { get; set; }
}
