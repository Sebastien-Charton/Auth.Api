﻿using Auth.Api.Application.Users.Commands.RegisterUser;
using Auth.Api.Application.Users.Queries.GetUserById;
using Auth.Api.Shared.Tests;
using Mailjet.Client;
using Newtonsoft.Json.Linq;
using ValidationException = Auth.Api.Application.Common.Exceptions.ValidationException;

namespace Auth.Api.Web.IntegrationTests.Users.Commands.RegisterUser;

public class RegisterUserTests : RegisterUserFixtures
{
    [Fact]
    public async Task RegisterUser_ShouldCreateUser_WhenUserIsValid()
    {
        // Arrange

        MailJetClientMock
            .Setup(x => x.PostAsync(It.IsAny<MailjetRequest>()))
            .ReturnsAsync(new MailjetResponse(true, 200, new JObject()));

        RegisterUserCommand? registerUserCommand = new Faker<RegisterUserCommand>()
            .RuleFor(x => x.PhoneNumber, f => f.Person.Phone)
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.UserName, f => f.Internet.UserName())
            .RuleFor(x => x.Password, f => f.Internet.GeneratePassword())
            .Generate();

        // Act

        var result = await SendAsync(registerUserCommand);

        // Assert

        var getUserByIdCommand = new GetUserByIdQuery { Id = result };
        await FluentActions.Invoking(() =>
            SendAsync(getUserByIdCommand)).Should().NotThrowAsync();

        MailJetClientMock
            .Verify(x => x.PostAsync(It.IsAny<MailjetRequest>()), Times.Once);
    }

    [Fact]
    public async Task RegisterUser_ShouldReturnValidationException_WhenUserIsInvalid()
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

        MailJetClientMock
            .Setup(x => x.PostAsync(It.IsAny<MailjetRequest>()))
            .ReturnsAsync(new MailjetResponse(true, 200, new JObject()));

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
            .ThrowAsync<ValidationException>();
    }
}