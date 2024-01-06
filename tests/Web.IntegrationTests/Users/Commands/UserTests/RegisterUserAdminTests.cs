using System.Net;
using System.Net.Http.Json;
using Auth.Api.Application.Users.Commands.RegisterUserAdmin;
using Auth.Api.Application.Users.Queries.GetUserById;
using Auth.Api.Domain.Constants;
using Mailjet.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Resource;

namespace Auth.Api.Web.IntegrationTests.Users.Commands.UserTests;

public class RegisterUserAdminAdminTests : UserEndpointsFixtures
{
    [Fact]
    public async Task RegisterUserAdmin_ShouldCreateUser_WhenUserIsValid()
    {
        // Arrange

        MailJetClientMock
            .Setup(x => x.PostAsync(It.IsAny<MailjetRequest>()))
            .ReturnsAsync(new MailjetResponse(true, 200, new JObject()));

        var registerUserAdminCommand = GenerateRegisterUserAdminCommand();
        registerUserAdminCommand.Roles = Roles.GetRoles;

        // Act

        var registerUserAdminResponse =
            await HttpClient.PostAsJsonAsync(RegisterUserAdminUri, registerUserAdminCommand);
        var registerUserAdminResult = await registerUserAdminResponse.Content.ReadFromJsonAsync<Guid>();
        // Assert

        registerUserAdminResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var getUserByIdResponse =
            await HttpClient.GetAsync(GetUserByIdUri + "/" + registerUserAdminResult);
        var getUserByIdResult = await getUserByIdResponse.Content.ReadFromJsonAsync<GetUserByIdResponse>();

        getUserByIdResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getUserByIdResult!.UserName.Should().Be(registerUserAdminCommand.UserName);
        getUserByIdResult.Email.Should().Be(registerUserAdminCommand.Email);

        MailJetClientMock
            .Verify(x => x.PostAsync(It.IsAny<MailjetRequest>()), Times.Once);
    }

    [Fact]
    public async Task RegisterUserAdmin_ShouldReturnValidationException_WhenUserIsInvalid()
    {
        // Arrange
        var registerUserAdminCommand = new RegisterUserAdminCommand();

        // Act
        var registerUserAdminResponse =
            await HttpClient.PostAsJsonAsync(RegisterUserAdminUri, registerUserAdminCommand);
        var registerUserAdminResult =
            await registerUserAdminResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        // Assert
        registerUserAdminResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        registerUserAdminResult!.Errors.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task RegisterUserAdmin_ShouldReturnValidationException_WhenUserAlreadyExists()
    {
        // Arrange

        MailJetClientMock
            .Setup(x => x.PostAsync(It.IsAny<MailjetRequest>()))
            .ReturnsAsync(new MailjetResponse(true, 200, new JObject()));

        var registerUserAdminCommand = GenerateRegisterUserAdminCommand();
        registerUserAdminCommand.Roles = Roles.GetRoles;

        await HttpClient.PostAsJsonAsync(RegisterUserAdminUri, registerUserAdminCommand);

        // Act
        var registerUserAdminResponse =
            await HttpClient.PostAsJsonAsync(RegisterUserAdminUri, registerUserAdminCommand);
        var registerUserAdminResult =
            await registerUserAdminResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        // Assert
        registerUserAdminResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        registerUserAdminResult!.Errors.Count.Should().BeGreaterThan(0);
        registerUserAdminResult.Errors.Values.Should().Contain(x =>
            x.Contains(UserErrorMessages.EmailAlreadyExists) || x.Contains(UserErrorMessages.UserNameAlreadyExists));
    }
}
