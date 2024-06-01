using Blog.Domain.Interfaces;
using Blog.SharedResources;
using Moq;
using Xunit;
using static Blog.Application.Features.Posts.Commands.DeletePost;

namespace Blog.UnitTests.Applications.Features.Posts.Commands;

public class DeletePostTests
{
    [Fact]
    public async Task Should_Have_Error_When_Repository_Returns_Failure()
    {
        int postId = 1;

        var postRepositoryMock = new Mock<IPostRepository>();
        postRepositoryMock.Setup(x => x.DeletePost(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(Error.Failure("Failure", ["Something went wrong, please try again later."])));

        var command = new DeletePostCommand(postId);
        var handler = new DeletePostCommandHandler(postRepositoryMock.Object);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task Should_Have_No_Error_When_Repository_Returns_Success()
    {
        int postId = 1;

        var postRepositoryMock = new Mock<IPostRepository>();
        postRepositoryMock.Setup(x => x.DeletePost(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(Success.NoContent()));

        var command = new DeletePostCommand(postId);
        var handler = new DeletePostCommandHandler(postRepositoryMock.Object);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.NotNull(result.Response);
        Assert.Equal(SuccessType.NoContent, result.Response.Type);
    }
}
