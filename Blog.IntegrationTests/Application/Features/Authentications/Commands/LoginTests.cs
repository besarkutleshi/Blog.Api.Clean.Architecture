using Blog.Application.Features.Auth.Dtos.Requests;
using Blog.Application.Features.Auth.Dtos.Responses;
using Blog.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using static Blog.Application.Features.Authentication.Commands.Login;

namespace Blog.IntegrationTests.Application.Features.Authentications.Commands;

public class LoginTests : BaseIntegrationTest
{
    private readonly IAuthenticationRepository _authenticationRepository;
    private readonly IAuthorizationRepository _authorizationRepository;

    public LoginTests(CustomWebApplicationFactory factory)
        : base(factory)
    {
        _authenticationRepository = serviceScope.ServiceProvider.GetRequiredService<IAuthenticationRepository>();
        _authorizationRepository = serviceScope.ServiceProvider.GetRequiredService<IAuthorizationRepository>();
    }

    [Fact]
    public async Task Should_Have_No_Error_When_Credentials_Are_Valid()
    {
        var user = await CreateUser();
        var loginDto = new LoginDto(user.Email!, _password);

        var command = new LoginCommand(loginDto);
        var handler = new LoginCommandHandler(_authenticationRepository, _authorizationRepository, _authTokenFactory);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Response);
        Assert.NotNull(result.Response.Result);
        Assert.IsType<UserInfoDto>(result.Response.Result);

        var userInfo = (UserInfoDto)result.Response.Result;

        Assert.NotEmpty(userInfo.Token);
    }
}
