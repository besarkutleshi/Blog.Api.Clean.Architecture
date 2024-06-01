using Blog.Application.Contracts;
using Blog.Application.Features.Auth.Dtos.Requests;
using Blog.Application.Features.Auth.Dtos.Responses;
using Blog.Domain.Interfaces;
using Blog.IntegrationTests.Factories;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using static Blog.Application.Features.Authentication.Commands.Login;

namespace Blog.IntegrationTests.Application.Features.Authentications.Commands;

public class LoginTests : BaseIntegrationTest
{
    private readonly IAuthenticationRepository _authenticationRepository;
    private readonly IAuthorizationRepository _authorizationRepository;
    private readonly IAuthTokenFactory _authTokenFactory;
    private readonly Faker<IdentityUser> _identityUserManger;
    private string _password = "Password.1";

    public LoginTests(CustomWebApplicationFactory factory)
        : base(factory)
    {
        _authenticationRepository = serviceScope.ServiceProvider.GetRequiredService<IAuthenticationRepository>();
        _authorizationRepository = serviceScope.ServiceProvider.GetRequiredService<IAuthorizationRepository>();
        _authTokenFactory = serviceScope.ServiceProvider.GetRequiredService<IAuthTokenFactory>();
        _identityUserManger = IdentityUserFakerFactory.Create();
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

    private async Task<IdentityUser> CreateUser()
    {
        var identityUser = _identityUserManger.Generate();
        var roleName = Guid.NewGuid().ToString() + "-Admin";

        var asdd = await _userManager.CreateAsync(identityUser, _password);
        var asd = await _roleManager.CreateAsync(new IdentityRole { Name = roleName });

        var addToRoleResult = await _userManager.AddToRoleAsync(identityUser, roleName);

        return identityUser;
    }
}
