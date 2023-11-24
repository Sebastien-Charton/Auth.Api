namespace Auth.Api.Web.IntegrationTests.Users;

public class UserEndpointsFixtures : TestingFixture
{
    public static readonly Uri RegisterUserUri = new(BaseUri + "User/register");
    public static readonly Uri LoginUserUri = new(BaseUri + "User/login");
    public static readonly Uri ConfirmEmailUri = new(BaseUri + "User/confirm-email");
    public static readonly Uri IsEmailConfirmedUri = new(BaseUri + "User/is-email-confirmed");
    public static readonly Uri GetUserByIdUri = new(BaseUri + "User");
}
