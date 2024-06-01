using Blog.Application.Contracts;
using Blog.Application.Features.Auth.Dtos.Requests;
using Blog.Application.Features.Auth.Dtos.Responses;
using Blog.Domain.Interfaces;
using Blog.SharedResources;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Text;
using System.Text.Json;

namespace Blog.IntegrationTests.Presentation.Controllers;

public class AuthControllerTests : BaseIntegrationTest
{
    private readonly HttpClient _client;
    private readonly Mock<IAuthorizationRepository> _authorizationRepository;
    private readonly Mock<IAuthenticationRepository> _authenticationRepository;
    private readonly Mock<IAuthTokenFactory> _authTokenRepository;

    public AuthControllerTests(CustomWebApplicationFactory factory)
        : base(factory)
    {
        _authenticationRepository = new Mock<IAuthenticationRepository>();
        _authorizationRepository = new Mock<IAuthorizationRepository>();
        _authTokenRepository = new Mock<IAuthTokenFactory>();

        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_authTokenRepository.Object);
                services.AddSingleton(_authorizationRepository.Object);
                services.AddSingleton(_authenticationRepository.Object);
            });
        }).CreateClient();
    }

    [Fact]
    public async Task Should_Have_No_Error_When_Credentials_Are_Valid()
    {
        MockRepositories();

        var loginDto = new LoginDto("email@email.com", "Password1.");

        var content = new StringContent(JsonSerializer.Serialize(loginDto), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/auth", content);

        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Should_Have_Error_When_Email_Is_Empty()
    {
        var loginDto = new LoginDto(string.Empty, "Password1.");

        var content = new StringContent(JsonSerializer.Serialize(loginDto), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/auth", content);

        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Should_Have_Error_When_Password_Is_Empty()
    {
        var loginDto = new LoginDto("email@email.com", string.Empty);

        var content = new StringContent(JsonSerializer.Serialize(loginDto), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/auth", content);

        Assert.False(response.IsSuccessStatusCode);
    }

    private void MockRepositories()
    {
        _authTokenRepository.Setup(x => x.GenerateToken(It.IsAny<UserInfoDto>()))
            .Returns("asdasdasdasda");
        
        _authorizationRepository.Setup(x => x.GetUserRoles(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(Success.Ok(new List<string> { "Admin" })));

        _authenticationRepository.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(Success.Ok(new IdentityUser { Id = Guid.NewGuid().ToString(), Email = "email@email.com"})));
    }
}
