using Auth.Api.Application.Users.Commands.ConfirmEmail;
using Auth.Api.Application.Users.Commands.IsEmailConfirmed;
using Auth.Api.Application.Users.Commands.RegisterUser;
using Shared.Tests;

namespace Auth.Api.Application.FunctionalTests.Users.Commands.ConfirmEmail;

public class ConfirmEmailTests : BaseTestFixture
{
    [Fact]
    public async Task RegisterUser_ShouldCreateUser_WhenUserIsValid()
    {
        // Arrange
        RegisterUserCommand? registerUserCommand = new Faker<RegisterUserCommand>()
            .RuleFor(x => x.PhoneNumber, f => f.Person.Phone)
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.UserName, f => f.Internet.UserName())
            .RuleFor(x => x.Password, f => f.Internet.GeneratePassword())
            .Generate();

        var userId = await SendAsync(registerUserCommand);

        var emailConfirmationToken = await GenerateConfirmationEmail(userId);

        emailConfirmationToken.Should().NotBeNull();
        emailConfirmationToken.Should().NotBeEmpty();

        var confirmEmailCommand = new ConfirmEmailCommand { Token = emailConfirmationToken!, UserId = userId };
        var isEmailConfirmedCommand = new IsEmailConfirmedCommand { UserId = userId };

        // Act

        var emailConfirmationResult = await SendAsync(confirmEmailCommand);
        var isEmailConfirmedResult = await SendAsync(isEmailConfirmedCommand);

        // Assert

        emailConfirmationResult.Should().BeTrue();
        isEmailConfirmedResult.Should().BeTrue();
    }
}
