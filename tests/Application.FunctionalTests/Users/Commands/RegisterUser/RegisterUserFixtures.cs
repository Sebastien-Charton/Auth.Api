using Mailjet.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Application.FunctionalTests.Users.Commands.RegisterUser;

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
