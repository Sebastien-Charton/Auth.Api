using System.Net;
using System.Net.Http.Json;
using Auth.Api.Application.Users.Commands.RegisterUser;
using Auth.Api.Application.Users.Queries.GetUserById;
using Mailjet.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Resource;

namespace Auth.Api.Web.IntegrationTests.Users.Commands;

public class RegisterUserTests : UserEndpointsFixtures
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

        registerUserResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var getUserByIdResponse =
            await HttpClient.GetAsync(GetUserByIdUri + "/" + registerUserResult);
        var getUserByIdResult = await getUserByIdResponse.Content.ReadFromJsonAsync<GetUserByIdResponse>();

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
        var registerUserCommand = new RegisterUserCommand();

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
        registerUserResult.Errors.Values.Should().Contain(x =>
            x.Contains(UserErrorMessages.EmailAlreadyExists) || x.Contains(UserErrorMessages.UserNameAlreadyExists));
    }
}
