using Blog.Application.Features.Auth.Dtos.Requests;
using Blog.Application.Features.Posts.Dtos.Requests;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Blog.Infrastructure.ConfigurationSections;
using Blog.SharedResources;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http.Json;

namespace Blog.IntegrationTests.Presentation.Controllers;

public class PostsControllerTests : BaseIntegrationTest
{
    private readonly HttpClient _httpClient;
    private readonly Mock<IPostRepository> _postRepository = new();
    private readonly IConfiguration _configuration;

    public PostsControllerTests(CustomWebApplicationFactory factory)
        : base(factory)
    {
        _configuration = serviceScope.ServiceProvider.GetRequiredService<IConfiguration>();
        _httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(_postRepository.Object);
            });
        }).CreateClient();
    }

    [Fact]
    public async Task Should_Have_Error_When_Creating_Post_With_Empty_Content()
    {
        await SignIn();

        var createPostDto = new CreatePostDto(string.Empty, "Valid Title", "Valid FriendlyUrl");

        var response = await _httpClient.PostAsJsonAsync("/api/posts", createPostDto);

        Assert.NotNull(response);
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Should_Have_Error_When_Creating_Post_With_Content_That_Exceeds_Maximum_Characters()
    {
        await SignIn();

        var createPostDto = new CreatePostDto(new string('a', 1001), "Valid Title", "Valid FriendlyUrl");

        var response = await _httpClient.PostAsJsonAsync("/api/posts", createPostDto);

        Assert.NotNull(response);
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Should_Have_Error_When_Creating_Post_With_Empty_Title()
    {
        await SignIn();

        var createPostDto = new CreatePostDto("Valid Content", string.Empty, "Valid FriendlyUrl");

        var response = await _httpClient.PostAsJsonAsync("/api/posts", createPostDto);

        Assert.NotNull(response);
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Should_Have_Error_When_Creating_Post_With_Title_That_Exceeds_Maximum_Characters()
    {
        await SignIn();

        var createPostDto = new CreatePostDto("Valid Content", new string('a', 101), "Valid FriendlyUrl");

        var response = await _httpClient.PostAsJsonAsync("/api/posts", createPostDto);

        Assert.NotNull(response);
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Should_Have_Error_When_Creating_Post_With_FriendlyUrl_That_Exceeds_Maximum_Characters()
    {
        await SignIn();

        var createPostDto = new CreatePostDto("Valid Content", "Valid Title", new string('a', 201));

        var response = await _httpClient.PostAsJsonAsync("/api/posts", createPostDto);

        Assert.NotNull(response);
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Should_Have_No_Error_When_Creating_Post()
    {
        _postRepository.Setup(x => x.CreatePost(It.IsAny<Post>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(Success.Created(new Post())));

        await SignIn();

        var createPostDto = new CreatePostDto("Valid Content", "Valid Title", "Valid FrinedlyUrl");

        var response = await _httpClient.PostAsJsonAsync("/api/posts", createPostDto);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Should_Have_Error_When_Deleting_Post_Id_Is_Lower_Or_Equal_With_0()
    {
        await SignIn();
     
        var postId = 0;

        var response = await _httpClient.DeleteAsync($"/api/posts/{postId}");

        Assert.NotNull(response);
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Should_Have_No_Error_When_Deleting_Post()
    {
        _postRepository.Setup(x => x.DeletePost(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(Success.NoContent()));

        await SignIn();

        var postId = 1;

        var response = await _httpClient.DeleteAsync($"/api/posts/{postId}");

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Should_Have_Error_When_Getting_Posts_PageIndex_Is_Lower_Or_Equal_With_0()
    {
        await SignIn();

        var pageIndex = 0;
        var pageSize = 10;

        var response = await _httpClient.GetAsync($"/api/posts/?pageIndex={pageIndex}&pageSize={pageSize}");

        Assert.NotNull(response);
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Should_Have_Error_When_Getting_Posts_PageSize_Is_Not_Between_1_And_100()
    {
        await SignIn();

        var pageIndex = 1;
        var pageSize = 200;

        var response = await _httpClient.GetAsync($"/api/posts/?pageIndex={pageIndex}&pageSize={pageSize}");

        Assert.NotNull(response);
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Should_Have_No_Error_When_Getting_Posts()
    {
        _postRepository.Setup(x => x.GetPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(Success.Ok(new List<Post>())));

        await SignIn();

        var pageIndex = 1;
        var pageSize = 10;

        var response = await _httpClient.GetAsync($"/api/posts/?pageIndex={pageIndex}&pageSize={pageSize}");

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Should_Have_Error_When_Updating_Post_With_Id_Lower_Or_Equal_With_0()
    {
        await SignIn();

        var updatePostDto = new UpdatePostDto(0, "Valid Title", "Valid Content", "Valid FriendlyUrl");

        var response = await _httpClient.PutAsJsonAsync("/api/posts", updatePostDto);

        Assert.NotNull(response);
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Should_Have_Error_When_Updating_Post_With_Empty_Content()
    {
        await SignIn();

        var updatePostDto = new UpdatePostDto(1, "Valid Title", string.Empty, "Valid FriendlyUrl");

        var response = await _httpClient.PutAsJsonAsync("/api/posts", updatePostDto);

        Assert.NotNull(response);
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Should_Have_Error_When_Updating_Post_With_Content_That_Exceeds_Maximum_Characters()
    {
        await SignIn();

        var updatePostDto = new UpdatePostDto(1, "Valid Title", new string('a', 1001), "Valid FriendlyUrl");

        var response = await _httpClient.PutAsJsonAsync("/api/posts", updatePostDto);

        Assert.NotNull(response);
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Should_Have_Error_When_Updating_Post_With_Empty_Title()
    {
        await SignIn();

        var updatePostDto = new UpdatePostDto(1, string.Empty, "Valid Content", "Valid FriendlyUrl");

        var response = await _httpClient.PutAsJsonAsync("/api/posts", updatePostDto);

        Assert.NotNull(response);
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Should_Have_Error_When_Updating_Post_With_Title_That_Exceeds_Maximum_Characters()
    {
        await SignIn();

        var updatePostDto = new UpdatePostDto(1, new string('a', 101), "Valid Content", "Valid FriendlyUrl");

        var response = await _httpClient.PutAsJsonAsync("/api/posts", updatePostDto);

        Assert.NotNull(response);
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Should_Have_Error_When_Updating_Post_With_FriendlyUrl_That_Exceeds_Maximum_Characters()
    {
        await SignIn();

        var updatePostDto = new UpdatePostDto(1, "Valid Title", "Valid Content", new string('a', 201));

        var response = await _httpClient.PutAsJsonAsync("/api/posts", updatePostDto);

        Assert.NotNull(response);
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Should_Have_No_Error_When_Updating_Post()
    {
        await SignIn();

        _postRepository.Setup(x => x.UpdatePost(It.IsAny<Post>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(Success.Ok()));

        var updatePostDto = new UpdatePostDto(1, "Valid Title", "Valid Content", "Valid FrinedlyUrl");

        var response = await _httpClient.PutAsJsonAsync("/api/posts", updatePostDto);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
    }

    private async Task SignIn()
    {
        var adminDefaultUser = new DefaultUser();
        _configuration.GetSection("AdminUser").Bind(adminDefaultUser);

        var loginDto = new LoginDto(adminDefaultUser.Email, adminDefaultUser.Password);

        var response = await _httpClient.PostAsJsonAsync("/api/auth", loginDto, CancellationToken.None);

        if (!response.IsSuccessStatusCode)
            throw new Exception(nameof(response));
    }
}
