using System.Net;
using System.Net.Http.Json;
using Auth.Api.Application.Users.Commands.RegisterUser;
using Auth.Api.Application.Users.Queries.GetUserById;
using Auth.Api.Shared.Tests;
using Mailjet.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Resource;
using ValidationException = Auth.Api.Application.Common.Exceptions.ValidationException;

namespace Auth.Api.Web.IntegrationTests.Users.Commands;

public class RegisterUserTests : RegisterUserTestsFixtures
{
    [Fact]
    public async Task RegisterUser_ShouldCreateUser_WhenUserIsValid()
    {
        // Arrange

        MailJetClientMock
            .Setup(x => x.PostAsync(It.IsAny<MailjetRequest>()))
            .ReturnsAsync(new MailjetResponse(true, 200, new JObject()));

        var registerUserCommand = GenerateRegisterUserCommand();

        // Act

        var registerUserResponse = await HttpClient.PostAsJsonAsync(RegisterUserUri, registerUserCommand);
        var registerUserResult = await registerUserResponse.Content.ReadFromJsonAsync<Guid>();
        // Assert

        registerUserResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var getUserByIdResponse =
            await HttpClient.GetAsync(GetUserByIdUri + "/" + registerUserResult);
        var getUserByIdResult = await getUserByIdResponse.Content.ReadFromJsonAsync<GetUserByIdDto>();
        
        getUserByIdResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getUserByIdResult!.UserName.Should().Be(registerUserCommand.UserName);
        getUserByIdResult.Email.Should().Be(registerUserCommand.Email);

        MailJetClientMock
            .Verify(x => x.PostAsync(It.IsAny<MailjetRequest>()), Times.Once);
    }

    [Fact]
    public async Task RegisterUser_ShouldReturnValidationException_WhenUserIsInvalid()
    {
        // Arrange
        var registerUserCommand = GenerateRegisterUserCommand();

        // Act
        var registerUserResponse = await HttpClient.PostAsJsonAsync(RegisterUserUri, registerUserCommand);
        var registerUserResult = await registerUserResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        
        // Assert
        registerUserResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        registerUserResult!.Errors.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task RegisterUser_ShouldReturnValidationException_WhenUserAlreadyExists()
    {
        // Arrange

        MailJetClientMock
            .Setup(x => x.PostAsync(It.IsAny<MailjetRequest>()))
            .ReturnsAsync(new MailjetResponse(true, 200, new JObject()));

        var registerUserCommand = GenerateRegisterUserCommand();

        await HttpClient.PostAsJsonAsync(RegisterUserUri, registerUserCommand);
        
        // Act
        var registerUserResponse = await HttpClient.PostAsJsonAsync(RegisterUserUri, registerUserCommand);
        var registerUserResult = await registerUserResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        
        // Assert
        registerUserResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        registerUserResult!.Errors.Count.Should().BeGreaterThan(0);
        registerUserResult!.Errors.Should().ContainValue(new []{UserErrorMessages.EmailAlreadyExists});
        registerUserResult!.Errors.Should().ContainValue(new []{UserErrorMessages.UserNameAlreadyExists});
    }
}
