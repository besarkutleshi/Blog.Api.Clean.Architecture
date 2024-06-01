using Blog.Domain.EntityErrors;
using Blog.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Blog.IntegrationTests.Infrastructure.Repositories;

public class AuthorizationRepositoryTests : BaseIntegrationTest
{
    public AuthorizationRepositoryTests(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_Have_Error_When_User_Does_Not_Exists()
    {
        string userId = Guid.NewGuid().ToString();

        var authorizationRepository = new AuthorizationRepository(_userManager);

        var result = await authorizationRepository.GetUserRoles(userId, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.NotNull(result.Error.ErrorMessages);
        Assert.Single(result.Error.ErrorMessages);
        Assert.Equal(UserErrors.UserNotFoundById(userId).ErrorMessages[0], result.Error.ErrorMessages[0]);
    }

    [Fact]
    public async Task Should_Have_Error_When_No_Roles_Is_Assigned()
    {
        string password = "Password1.";

        var identityUser = _userFaker.Generate();

        await _userManager.CreateAsync(identityUser, password);

        var authorizationRepository = new AuthorizationRepository(_userManager);

        var result = await authorizationRepository.GetUserRoles(identityUser.Id, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.NotNull(result.Error.ErrorMessages);
        Assert.Single(result.Error.ErrorMessages);
        Assert.Equal(UserErrors.NoRoleAssigned().ErrorMessages[0], result.Error.ErrorMessages[0]);
    }

    [Fact]
    public async Task Should_Have_No_Error_When_User_And_Roles_Are_Valid()
    {
        string password = "Password1.";
        string roleName = $"Admin-{Guid.NewGuid()}";

        var identityUser = _userFaker.Generate();

        var identityRole = new IdentityRole
        {
            Name = roleName
        };

        await _userManager.CreateAsync(identityUser, password);
        await _roleManager.CreateAsync(identityRole);
        await _userManager.AddToRoleAsync(identityUser, roleName);

        var authorizationRepository = new AuthorizationRepository(_userManager);

        var result = await authorizationRepository.GetUserRoles(identityUser.Id, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Response);
        Assert.NotNull(result.Response.Result);
    }
}
