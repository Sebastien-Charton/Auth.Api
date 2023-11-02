using Auth.Api.Infrastructure.Options;
using Auth.Api.Infrastructure.ServiceAgents;
using FluentAssertions;
using Mailjet.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Infrastructure.UnitTests.ServiceAgents;

public class MailJetServiceAgentTests
{
    [Fact]
    public async Task SendMail_ShouldReturnTrue_WhenDataAreValid()
    {
        // Arrange
        var mailJetClient = new Mock<IMailjetClient>();
        var logger = new Mock<ILogger<MailJetServiceAgent>>();

        var mailJetResponse = new MailjetResponse(true, 200, new JObject());
        var mailJetOptions = Options.Create(new MailJetOptions());

        mailJetClient
            .Setup(x => x.PostAsync(It.IsAny<MailjetRequest>()))
            .ReturnsAsync(mailJetResponse);

        var mailJetServiceAgent = new MailJetServiceAgent(logger.Object, mailJetOptions, mailJetClient.Object);
        // Act

        var response = await mailJetServiceAgent.SendMail("toEmail", "toName", "subject", "text", "html");

        // Assert

        response.Should().BeTrue();
    }
}
