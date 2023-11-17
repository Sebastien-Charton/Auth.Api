using Auth.Api.Application.Users.Commands.RegisterUser;
using Auth.Api.Shared.Tests;

namespace Auth.Api.Web.IntegrationTests.Users;

public class UserEndpointsFixtures : TestingFixture
{
    public static readonly Uri RegisterUserUri = new(BaseUri + "UsersEndpoints/register");
    public static readonly Uri LoginUserUri = new(BaseUri + "UsersEndpoints/login");
    public static readonly Uri ConfirmEmailUri = new(BaseUri + "UsersEndpoints/confirm-email");
    public static readonly Uri IsEmailConfirmedUri = new(BaseUri + "UsersEndpoints/is-email-confirmed");
    public static readonly Uri GetUserByIdUri = new(BaseUri + $"api/api/UsersEndpoints/user");

    public RegisterUserCommand GenerateRegisterUserCommand()
    {
        return new Faker<RegisterUserCommand>()
            .RuleFor(x => x.PhoneNumber, f => f.Person.Phone)
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.UserName, f => f.Internet.UserName())
            .RuleFor(x => x.Password, f => f.Internet.GeneratePassword())
            .Generate();
    }
}
