using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Application.Users.Commands.ConfirmEmail;
using Auth.Api.Application.Users.Commands.IsEmailConfirmed;
using Auth.Api.Application.Users.Commands.RegisterUser;
using Auth.Api.Shared.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace Web.IntegrationTests.Users.Commands.IsEmailConfirmed;

public class IsEmailConfirmedTests : TestingFixture
{
    [Fact]
    public async Task IsEmailConfirmed_ShouldReturnTrue_WhenEmailIsConfirmed()
    {
        // Arrange
        RegisterUserCommand? registerUserCommand = new Faker<RegisterUserCommand>()
            .RuleFor(x => x.PhoneNumber, f => f.Person.Phone)
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.UserName, f => f.Internet.UserName())
            .RuleFor(x => x.Password, f => f.Internet.GeneratePassword())
            .Generate();

        var userId = await SendAsync(registerUserCommand);

        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        var emailConfirmationToken = await userManagerService.GenerateEmailConfirmationToken(userId);

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
