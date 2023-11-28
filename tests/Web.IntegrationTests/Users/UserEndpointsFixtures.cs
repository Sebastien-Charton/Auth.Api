using Mailjet.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Web.IntegrationTests.Users;

public class UserEndpointsFixtures : TestingFixture
{
    protected static readonly Uri RegisterUserUri = new(BaseUri + "User/register");
    protected static readonly Uri RegisterUserAdminUri = new(BaseUri + "User/register-admin");
    protected static readonly Uri LoginUserUri = new(BaseUri + "User/login");
    protected static readonly Uri ConfirmEmailUri = new(BaseUri + "User/confirm-email");
    protected static readonly Uri IsEmailConfirmedUri = new(BaseUri + "User/is-email-confirmed");
    protected static readonly Uri GetUserByIdUri = new(BaseUri + "User");
    protected static readonly Uri GetEmailConfirmationTokenUri = new(BaseUri + "User/confirmation-email-token");
    protected static readonly Uri IsEmailExistsUri = new(BaseUri + "User/is-email-exists");
    protected static readonly Uri IsUserNameExistsUri = new(BaseUri + "User/is-username-exists");
    protected static readonly Uri GetPasswordResetTokenUri = new(BaseUri + "User/password-reset-token");
    protected static readonly Uri UpdatePasswordUri = new(BaseUri + "User/update-password");
    protected static readonly Uri ResetPasswordUri = new(BaseUri + "User/reset-password");
    protected static readonly Uri DeleteUserUri = new(BaseUri + "User");
    protected static readonly Uri SendEmailConfirmationTokenUri = new(BaseUri + "User/send-confirmation-email-token");
    protected static readonly Uri SendPasswordResetTokenUri = new(BaseUri + "User/send-password-reset-token");

    protected UserEndpointsFixtures()
    {
        MailJetClientMock = new Mock<IMailjetClient>();
    }

    protected Mock<IMailjetClient> MailJetClientMock { get; }

    protected override ServiceDescriptor[] ConfigureMocks()
    {
        return new[] { ServiceDescriptor.Scoped(_ => MailJetClientMock.Object) };
    }
}
