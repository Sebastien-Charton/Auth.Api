using Mailjet.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Web.IntegrationTests.Users.Commands.RegisterUser;

public class RegisterUserFixtures : TestingFixture
{
    protected RegisterUserFixtures()
    {
        MailJetClientMock = new Mock<IMailjetClient>();
    }

    protected Mock<IMailjetClient> MailJetClientMock { get; }

    protected override ServiceDescriptor[] ConfigureMocks()
    {
        return new[] { ServiceDescriptor.Scoped(x => MailJetClientMock.Object) };
    }
}
