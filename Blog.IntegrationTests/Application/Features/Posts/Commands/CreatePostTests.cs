using Blog.Application.Features.Posts.Dtos.Requests;
using Blog.Application.Features.Posts.Dtos.Responses;
using Blog.Domain.Interfaces;
using Blog.IntegrationTests.Factories;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using static Blog.Application.Features.Posts.Commands.CreatePost;

namespace Blog.IntegrationTests.Application.Features.Posts.Commands;

public class CreatePostTests : BaseIntegrationTest
{
    private readonly IPostRepository _postRepository;

    public CreatePostTests(CustomWebApplicationFactory factory)
        : base(factory)
    {
        _postRepository = serviceScope.ServiceProvider.GetRequiredService<IPostRepository>();
    }

    [Fact]
    public async Task Should_Have_No_Error_When_Creating_Post()
    {
        var user = await CreateUser();

        var post = CreatePostFakerFactory.Create();
        _requestService.SetUserId(user.Id);

        var command = new CreatePostCommand(post);
        var handler = new CreatePostCommandHandler(_postRepository, _mapper, _requestService);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Response);
        Assert.NotNull(result.Response.Result);
        Assert.IsType<PostDto>(result.Response.Result);
    }
}
