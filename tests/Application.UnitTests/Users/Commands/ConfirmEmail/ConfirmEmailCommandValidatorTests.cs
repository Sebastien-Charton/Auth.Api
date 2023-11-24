using Auth.Api.Application.Users.Commands.ConfirmEmail;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Auth.Api.Application.UnitTests.Users.Commands.ConfirmEmail;

public class ConfirmEmailCommandValidatorTests
{
    [Fact]
    public void ConfirmEmailCommandValidator_ShouldBeValid_WhenDataAreCorrect()
    {
        // Arrange

        var confirmEmailCommand = new Faker<ConfirmEmailCommand>()
            .RuleFor(x => x.Token, f => f.Random.Guid().ToString())
            .Generate();

        var validator = new ConfirmEmailCommandValidator();

        // Act

        var result = validator.Validate(confirmEmailCommand);

        // Assert

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ConfirmEmailCommandValidator_ShouldBeInvalid_WhenDataAreIncorrect()
    {
        // Arrange

        var confirmEmailCommand = new ConfirmEmailCommand();

        var validator = new ConfirmEmailCommandValidator();

        // Act

        var result = validator.Validate(confirmEmailCommand);

        // Assert

        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
    }
}
