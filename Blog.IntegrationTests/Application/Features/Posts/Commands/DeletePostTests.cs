using Blog.Application.Helpers;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Blog.IntegrationTests.Factories;
using Blog.SharedResources;
using Bogus;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using static Blog.Application.Features.Posts.Commands.DeletePost;

namespace Blog.IntegrationTests.Application.Features.Posts.Commands;

public class DeletePostTests : BaseIntegrationTest
{
    private readonly IPostRepository _postRepository;
    private readonly Faker<Post> _postFaker;

    public DeletePostTests(CustomWebApplicationFactory factory)
        : base(factory)
    {
        _postRepository = serviceScope.ServiceProvider.GetRequiredService<IPostRepository>();
        _postFaker = PostFakerFactory.Create();
    }

    [Fact]
    public async Task Should_Have_No_Error_When_Deleting_Post()
    {
        var post = await CreatePost();

        var command = new DeletePostCommand(post.Id);
        var handler = new DeletePostCommandHandler(_postRepository);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Response);
        Assert.Equal(SuccessType.NoContent, result.Response.Type);
    }

    private async Task<Post> CreatePost()
    {
        var user = await CreateUser();

        var post = _postFaker.Generate();
        post.CreatedBy = user.Id;

        var result = await _postRepository.CreatePost(post, It.IsAny<CancellationToken>());

        return (Post)result.Response.Result!;
    }
}
