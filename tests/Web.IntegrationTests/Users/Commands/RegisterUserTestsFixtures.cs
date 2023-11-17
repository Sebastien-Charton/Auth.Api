using Auth.Api.Infrastructure.Options;
using Mailjet.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Web.IntegrationTests.Users.Commands;

public class RegisterUserTestsFixtures : UserEndpointsFixtures
{
    protected RegisterUserTestsFixtures()
    {
        MailJetClientMock = new Mock<IMailjetClient>();
    }

    protected Mock<IMailjetClient> MailJetClientMock { get; }

    protected override ServiceDescriptor[] ConfigureMocks()
    {
        return new[] { ServiceDescriptor.Scoped(x => MailJetClientMock.Object) };
    }
}
