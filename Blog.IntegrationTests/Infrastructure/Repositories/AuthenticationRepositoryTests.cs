using Blog.Domain.EntityErrors;
using Blog.Infrastructure.Repositories;
using Blog.SharedResources;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Blog.IntegrationTests.Infrastructure.Repositories;

public class AuthenticationRepositoryTests
    : BaseIntegrationTest
{
    public AuthenticationRepositoryTests(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_Have_Error_When_User_Not_Found_By_Email()
    {
        var email = $"email-{Guid.NewGuid()}@email.com";
        var password = "Password.2024";

        var authRepository = new AuthenticationRepository(_userManager, _signInManager);

        var result = await authRepository.Login(email, password, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
        Assert.NotNull(result.Error);
        Assert.NotNull(result.Error.ErrorMessages);
        Assert.Single(result.Error.ErrorMessages);
        Assert.Equal(UserErrors.UserNotFound(email).ErrorMessages[0], result.Error.ErrorMessages[0]);
    }

    [Fact]
    public async Task Should_Have_Error_When_Password_Is_Incorrect()
    {
        var password = "Password.2024";
        var incorrectPassword = "TestTest";

        var identityUser = _userFaker.Generate();

        await _userManager.CreateAsync(identityUser, password);

        var authRepository = new AuthenticationRepository(_userManager, _signInManager);

        var result = await authRepository.Login(identityUser.Email!, incorrectPassword, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
        Assert.NotNull(result.Error);
        Assert.NotNull(result.Error.ErrorMessages);
        Assert.Single(result.Error.ErrorMessages);
        Assert.Equal(UserErrors.IncorrectPasswrod().ErrorMessages[0], result.Error.ErrorMessages[0]);
    }

    [Fact]
    public async Task Should_Have_No_Error_When_Credentials_Are_Correct()
    {
        var password = "Password.2024";

        var identityUser = _userFaker.Generate();

        await _userManager.CreateAsync(identityUser, password);

        var authRepository = new AuthenticationRepository(_userManager, _signInManager);

        var result = await authRepository.Login(identityUser.Email!, password, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Response);
        Assert.NotNull(result.Response.Result);
        Assert.IsType<IdentityUser>(result.Response.Result);
    }
}
