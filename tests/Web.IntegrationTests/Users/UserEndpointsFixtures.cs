﻿namespace Auth.Api.Web.IntegrationTests.Users;

public class UserEndpointsFixtures : TestingFixture
{
    protected static readonly Uri RegisterUserUri = new(BaseUri + "User/register");
    protected static readonly Uri RegisterUserAdminUri = new(BaseUri + "User/register-admin");
    protected static readonly Uri LoginUserUri = new(BaseUri + "User/login");
    protected static readonly Uri ConfirmEmailUri = new(BaseUri + "User/confirm-email");
    protected static readonly Uri IsEmailConfirmedUri = new(BaseUri + "User/is-email-confirmed");
    protected static readonly Uri GetUserByIdUri = new(BaseUri + "User");
    protected static readonly Uri GetEmailConfirmationToken = new(BaseUri + "User/confirmation-email-token");
    protected static readonly Uri IsEmailExistsUri = new(BaseUri + "User/is-email-exists");
    protected static readonly Uri IsUserNameExistsUri = new(BaseUri + "User/is-username-exists");
}
