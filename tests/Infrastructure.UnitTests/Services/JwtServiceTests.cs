﻿using System.IdentityModel.Tokens.Jwt;
using Auth.Api.Infrastructure.Identity.Models;
using Auth.Api.Infrastructure.Services;
using Auth.Api.Infrastructure.Settings;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Infrastructure.UnitTests.Services;

public class JwtServiceTests
{
    [Fact]
    public void GenerateJwtToken_ShouldReturnValidToken_WhenUserIsValid()
    {
        // Arrange

        var user = new Faker<ApplicationUser>()
            .RuleFor(x => x.Id, f => f.Random.Guid())
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.UserName, f => f.Internet.UserName())
            .Generate();

        var jwtConfiguration = new Faker<JwtConfiguration>()
            .RuleFor(x => x.Audience, f => f.Internet.Url())
            .RuleFor(x => x.Issuer, f => f.Internet.Url())
            .RuleFor(x => x.SecurityKey, f => f.Random.String(64))
            .RuleFor(x => x.ExpiryInDays, f => f.Random.Int(1, 30))
            .Generate();

        var jwtService = new JwtService(jwtConfiguration);

        // Act

        var result = jwtService.GenerateJwtToken(user, null);

        // Assert

        result.Should().NotBeNull();

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(result);
        var token = jsonToken as JwtSecurityToken;

        token.Should().NotBeNull();
        token!.Issuer.Should().Be(jwtConfiguration.Issuer);
        token.Audiences.FirstOrDefault().Should().Be(jwtConfiguration.Audience);
        token.Claims.Should().ContainSingle(x => x.Value == user.Id.ToString());
        token.Claims.Should().ContainSingle(x => x.Value == user.Email);
        token.Claims.Should().ContainSingle(x => x.Value == user.UserName);
        token.ValidTo.Should().Be(token.ValidFrom.AddDays(jwtConfiguration.ExpiryInDays).AddHours(1));
    }
}
