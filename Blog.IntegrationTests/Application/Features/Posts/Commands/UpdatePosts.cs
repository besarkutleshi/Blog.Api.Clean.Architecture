using Blog.Application.Features.Posts.Dtos.Requests;
using Blog.Application.Features.Posts.Dtos.Responses;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Blog.IntegrationTests.Factories;
using Blog.IntegrationTests.Helpers;
using Bogus;
using Bogus.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using static Blog.Application.Features.Posts.Commands.UpdatePost;

namespace Blog.IntegrationTests.Application.Features.Posts.Commands;

public class UpdatePosts : BaseIntegrationTest
{
    private readonly IPostRepository _postRepository;
    private readonly Faker<Post> _postFactory;

    public UpdatePosts(CustomWebApplicationFactory factory)
        : base(factory)
    {
        _postRepository = serviceScope.ServiceProvider.GetRequiredService<IPostRepository>();
        _postFactory = PostFakerFactory.Create();
    }

    [Fact]
    public async Task Should_Have_No_Error_When_Updating_Post()
    {
        var post = await CreatePost();

        var title = RandomStringGenerator.GenerateRandomString(10, 90);
        var content = RandomStringGenerator.GenerateRandomString(10, 90);
        var friendlyUrl = RandomStringGenerator.GenerateRandomString(10, 90);
        var updateDto = new UpdatePostDto(post.Id, title, content, friendlyUrl);

        var command = new UpdatePostCommand(updateDto);
        var handler = new UpdatePostCommandHandler(_postRepository, _mapper);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Response);
        Assert.NotNull(result.Response.Result);

        var postInfo = (PostDto)result.Response.Result;

        Assert.Equal(title, postInfo.Title);
        Assert.Equal(content, postInfo.Content);
        Assert.Equal(friendlyUrl, postInfo.FriendlyUrl);
    }

    private async Task<Post> CreatePost()
    {
        var user = await CreateUser();

        var post = _postFactory.Generate();
        post.CreatedBy = user.Id;

        var result = await _postRepository.CreatePost(post, It.IsAny<CancellationToken>());

        return (Post)result.Response.Result!;
    }
}
