namespace Auth.Api.Web.IntegrationTests.Users;

public class UserEndpointsFixtures : TestingFixture
{
    public static readonly Uri RegisterUserUri = new(BaseUri + "UsersEndpoints/register");
    public static readonly Uri LoginUserUri = new(BaseUri + "UsersEndpoints/login");
    public static readonly Uri ConfirmEmailUri = new(BaseUri + "UsersEndpoints/confirm-email");
    public static readonly Uri IsEmailConfirmedUri = new(BaseUri + "UsersEndpoints/is-email-confirmed");
}
