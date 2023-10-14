using Auth.Api.Application.Users.Commands.RegisterUser;
using Auth.Api.Application.Users.Queries.GetUserById;
using Shared.Tests;
using ValidationException = Auth.Api.Application.Common.Exceptions.ValidationException;

namespace Auth.Api.Application.FunctionalTests.Users.Commands.RegisterUser;

public class RegisterUserTests : BaseTestFixture
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

        // Act

        var result = await SendAsync(registerUserCommand);

        // Assert

        await FluentActions.Invoking(() =>
            SendAsync(new GetUserByIdCommand() { Id = result })).Should().NotThrowAsync();
    }

    [Fact]
    public async Task RegisterUser_ShouldReturnValidationException_WhenUserIsInValid()
    {
        // Arrange
        RegisterUserCommand? registerUserCommand = new Faker<RegisterUserCommand>()
            .RuleFor(x => x.PhoneNumber, f => f.Person.Phone)
            .RuleFor(x => x.Email, f => f.Person.FirstName)
            .RuleFor(x => x.UserName, f => f.Internet.UserName())
            .RuleFor(x => x.Password, f => f.Internet.GeneratePassword())
            .Generate();

        // Act

        // Assert

        await FluentActions.Invoking(() =>
            SendAsync(registerUserCommand)).Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task RegisterUser_ShouldReturnValidationException_WhenUserAlreadyExists()
    {
        // Arrange
        RegisterUserCommand? registerUserCommand = new Faker<RegisterUserCommand>()
            .RuleFor(x => x.PhoneNumber, f => f.Person.Phone)
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.UserName, f => f.Internet.UserName())
            .RuleFor(x => x.Password, f => f.Internet.GeneratePassword())
            .Generate();

        // Act

        Guid result = await SendAsync(registerUserCommand);

        // Assert

        await FluentActions.Invoking(() =>
                SendAsync(registerUserCommand)).Should()
            .ThrowAsync<System.ComponentModel.DataAnnotations.ValidationException>();
    }
}
