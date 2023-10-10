using Auth.Api.Application.Users.Commands.RegisterUser;
using Microsoft.Extensions.DependencyInjection.Users.Queries.GetUserById;

namespace Auth.Api.Application.FunctionalTests.Users.Commands.RegisterUser;

public class RegisterUserTests : BaseTestFixture
{
    [Fact]
    public async Task RegisterUser_ShouldCreateUser_WhenUserIsValid()
    {
        // Arrange

        var registerUserCommand = new RegisterUserCommand()
        {
            Email = "example@gmail.com", Password = "Password1*,", UserName = "TEST", PhoneNumber = "+35905458484"
        };
        
        // Act
        
        var result = await SendAsync(registerUserCommand);

        // Assert

        await FluentActions.Invoking(() =>
            SendAsync(new GetUserByIdCommand() { Id = result })).Should().NotThrowAsync();
    }
}
