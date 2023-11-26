using System.Net;
using System.Net.Http.Json;
using Auth.Api.Domain.Constants;
using Mailjet.Client;
using Newtonsoft.Json.Linq;

namespace Auth.Api.Web.IntegrationTests.Users.Commands;

public class DeleteUserTests : UserEndpointsFixtures
{
    [Fact]
    public async Task DeleteUser_ShouldDeleteUser_WhenUserExists()
    {
        // Arrange

        MailJetClientMock
            .Setup(x => x.PostAsync(It.IsAny<MailjetRequest>()))
            .ReturnsAsync(new MailjetResponse(true, 200, new JObject()));

        var registerUserAdminCommand = GenerateRegisterUserAdminCommand();
        registerUserAdminCommand.Roles = Roles.GetRoles;

        var registerUserAdminResponse =
            await HttpClient.PostAsJsonAsync(RegisterUserAdminUri, registerUserAdminCommand);
        var registerUserAdminResult = await registerUserAdminResponse.Content.ReadFromJsonAsync<Guid>();

        // Act

        var deleteUserResponse = await HttpClient.DeleteAsync(DeleteUserUri + "/" + registerUserAdminResult);
        var getUserByIdResponse = await HttpClient.GetAsync(GetUserByIdUri + "/" + registerUserAdminResult);
        // Assert

        deleteUserResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        getUserByIdResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
