using Blog.Application.Features.Posts.Dtos.Requests;
using Blog.Application.Features.Posts.Dtos.Responses;
using Blog.Application.Helpers;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Blog.SharedResources;
using MapsterMapper;
using Moq;
using Xunit;
using static Blog.Application.Features.Posts.Commands.CreatePost;

namespace Blog.UnitTests.Applications.Features.Posts.Commands;

public class CreatePostTests
{
    [Fact]
    public async Task Should_Have_Error_When_Repository_Returns_Failure_Result()
    {
        var dto = new CreatePostDto("Valid Contect", "Valid Title", "Valid FriendlyUrl");

        var postRepositoryMock = new Mock<IPostRepository>();
        postRepositoryMock.Setup(x => x.CreatePost(It.IsAny<Post>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(Error.Failure("Failure", ["Something went wrong, please try again later."])));

        var mapperMock = new Mock<Mapper>();
        mapperMock.Setup(x => x.Map<Post>(It.IsAny<CreatePostDto>()))
            .Returns(new Post());

        var requestServiceMock = new Mock<IRequestService>();
        requestServiceMock.Setup(x => x.UserId)
            .Returns("3fa85f64-5717-4562-b3fc-2c963f66afa4");

        var command = new CreatePostCommand(dto);
        var handler = new CreatePostCommandHandler(postRepositoryMock.Object, mapperMock.Object, requestServiceMock.Object);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task Should_Have_No_Error_When_Repository_And_Create_Model_Are_Valid()
    {
        var dto = new CreatePostDto("Valid Contect", "Valid Title", "Valid FriendlyUrl");
        var post = new Post();

        var postRepositoryMock = new Mock<IPostRepository>();
        postRepositoryMock.Setup(x => x.CreatePost(It.IsAny<Post>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(Success.Created(post)));

        var mapperMock = new Mock<Mapper>();
        mapperMock.Setup(x => x.Map<Post>(It.IsAny<CreatePostDto>()))
            .Returns(post);
        mapperMock.Setup(x => x.Map<PostDto>(It.IsAny<Post>()))
            .Returns(new PostDto(1, "Valid Title", "Valid Content", "Valid FriendlyUrl", Guid.NewGuid().ToString(), DateTime.UtcNow));

        var requestServiceMock = new Mock<IRequestService>();
        requestServiceMock.Setup(x => x.UserId)
            .Returns(Guid.NewGuid().ToString());

        var command = new CreatePostCommand(dto);
        var handler = new CreatePostCommandHandler(postRepositoryMock.Object, mapperMock.Object, requestServiceMock.Object);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.NotNull(result.Response);
        Assert.Equal(SuccessType.Created, result.Response.Type);
        Assert.IsType<PostDto>(result.Response.Result);
    }
}
