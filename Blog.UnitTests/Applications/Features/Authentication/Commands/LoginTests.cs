using Blog.Application.Contracts;
using Blog.Application.Features.Auth.Dtos.Requests;
using Blog.Application.Features.Auth.Dtos.Responses;
using Blog.Domain.EntityErrors;
using Blog.Domain.Interfaces;
using Blog.SharedResources;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;
using static Blog.Application.Features.Authentication.Commands.Login;

namespace Blog.UnitTests.Applications.Features.Authentication.Commands;

public class LoginTests
{
    [Fact]
    public async Task Should_Have_Error_When_Credentials_Are_Wrong()
    {
        string email = "email@email.com";
        string password = "password";

        var authenticationRepositoryMock = new Mock<IAuthenticationRepository>();
        authenticationRepositoryMock.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(UserErrors.UserNotFound(email)));

        var command = new LoginCommand(new LoginDto(email, password));
        var handler = new LoginCommandHandler(authenticationRepositoryMock.Object, null!, null!);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());
        
        Assert.NotNull(result);
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.NotNull(result.Error.ErrorMessages);
        Assert.Single(result.Error.ErrorMessages);
        Assert.Equal(UserErrors.UserNotFound(email).ErrorMessages[0], result.Error.ErrorMessages[0]);
    }

    [Fact]
    public async Task Should_Have_Error_When_There_Is_No_Role_Assigned()
    {
        string email = "email@email.com";
        string password = "password";

        var authenticationRepositoryMock = new Mock<IAuthenticationRepository>();
        authenticationRepositoryMock.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(Success.Ok(new IdentityUser { Email = email, Id = Guid.NewGuid().ToString() })));

        var authorizationRepositoryMock = new Mock<IAuthorizationRepository>();
        authorizationRepositoryMock.Setup(x => x.GetUserRoles(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(UserErrors.NoRoleAssigned()));

        var command = new LoginCommand(new LoginDto(email, password));
        var handler = new LoginCommandHandler(authenticationRepositoryMock.Object, authorizationRepositoryMock.Object, null!);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.NotNull(result.Error.ErrorMessages);
        Assert.Single(result.Error.ErrorMessages);
        Assert.Equal(UserErrors.NoRoleAssigned().ErrorMessages[0], result.Error.ErrorMessages[0]);
    }

    [Fact]
    public async Task Should_Have_Error_When_Token_Fails_To_Generate()
    {
        string email = "email@email.com";
        string password = "password";

        var authenticationRepositoryMock = new Mock<IAuthenticationRepository>();
        authenticationRepositoryMock.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(Success.Ok(new IdentityUser { Email = email, Id = Guid.NewGuid().ToString() })));

        var roles = new List<string> { "Admin" };
        var authorizationRepositoryMock = new Mock<IAuthorizationRepository>();
        authorizationRepositoryMock.Setup(x => x.GetUserRoles(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(Success.Ok(roles)));

        string? tokenResponse = null;
        var authTokenFactoryMock = new Mock<IAuthTokenFactory>();
        authTokenFactoryMock.Setup(x => x.GenerateToken(It.IsAny<UserInfoDto>()))
            .Returns(tokenResponse);

        var command = new LoginCommand(new LoginDto(email, password));
        var handler = new LoginCommandHandler(authenticationRepositoryMock.Object, authorizationRepositoryMock.Object, authTokenFactoryMock.Object);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.NotNull(result.Error.ErrorMessages);
        Assert.Single(result.Error.ErrorMessages);
        Assert.Equal("Failed to generate auth token.", result.Error.ErrorMessages[0]);
    }

    [Fact]
    public async Task Should_Have_No_Error_When_Credentials_Roles_And_Token_Are_Valid()
    {
        string email = "email@email.com";
        string password = "password";

        var authenticationRepositoryMock = new Mock<IAuthenticationRepository>();
        authenticationRepositoryMock.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(Success.Ok(new IdentityUser { Email = email, Id = Guid.NewGuid().ToString() })));

        var roles = new List<string> { "Admin" };
        var authorizationRepositoryMock = new Mock<IAuthorizationRepository>();
        authorizationRepositoryMock.Setup(x => x.GetUserRoles(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(Success.Ok(roles)));

        string? tokenResponse = "Token";
        var authTokenFactoryMock = new Mock<IAuthTokenFactory>();
        authTokenFactoryMock.Setup(x => x.GenerateToken(It.IsAny<UserInfoDto>()))
            .Returns(tokenResponse);

        var command = new LoginCommand(new LoginDto(email, password));
        var handler = new LoginCommandHandler(authenticationRepositoryMock.Object, authorizationRepositoryMock.Object, authTokenFactoryMock.Object);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.NotNull(result.Response);
        Assert.NotNull(result.Response.Result);
        Assert.IsType<UserInfoDto>(result.Response.Result);
    }
}
