using Blog.Application.Features.Auth.Dtos.Requests;
using Blog.Application.Features.Authentication.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;
using static Blog.Application.Features.Authentication.Commands.Login;

namespace Blog.UnitTests.Applications.Features.Authentication.Validators;

public class LoginValidatorTests
{
    private readonly LoginValidator _validator;

    public LoginValidatorTests()
    {
        _validator = new LoginValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var loginCommand = new LoginCommand(new LoginDto(string.Empty, "ValidPassword"));

        var result = _validator.TestValidate(loginCommand);

        var errors = result.ShouldHaveValidationErrorFor(x => x.LoginDto.Email)
            .WithErrorMessage("Email should not be empty");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Exceeds_Maximum_Length()
    {
        var loginCommand = new LoginCommand(new LoginDto(new string('a', 51), "ValidPassword"));

        var result = _validator.TestValidate(loginCommand);

        var errors = result.ShouldHaveValidationErrorFor(x => x.LoginDto.Email)
            .WithErrorMessage("Email cannot have more than 50 characters");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Empty()
    {
        var loginCommand = new LoginCommand(new LoginDto("valid@gmail.com", string.Empty));

        var result = _validator.TestValidate(loginCommand);

        var errors = result.ShouldHaveValidationErrorFor(x => x.LoginDto.Password)
            .WithErrorMessage("Password should not be empty");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Exceeds_Maximum_Length()
    {
        var loginCommand = new LoginCommand(new LoginDto("valid@gmail.com", new string('a', 51)));

        var result = _validator.TestValidate(loginCommand);

        var errors = result.ShouldHaveValidationErrorFor(x => x.LoginDto.Password)
            .WithErrorMessage("Password cannot have more than 50 characters");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Email_And_Password_Are_Valid()
    {
        var loginCommand = new LoginCommand(new LoginDto("valid@gmail.com", "ValidPassword"));

        var result = _validator.TestValidate(loginCommand);

        result.ShouldNotHaveValidationErrorFor(x => x.LoginDto.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.LoginDto.Password);
    }
}
