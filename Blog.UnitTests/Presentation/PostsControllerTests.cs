using Blog.Application.Features.Posts.Dtos.Requests;
using Blog.Application.Features.Posts.Dtos.Responses;
using Blog.Presentation.Controllers;
using Blog.SharedResources;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static Blog.Application.Features.Posts.Commands.CreatePost;
using static Blog.Application.Features.Posts.Commands.DeletePost;
using static Blog.Application.Features.Posts.Commands.ImportPostsFromExcel;
using static Blog.Application.Features.Posts.Commands.UpdatePost;
using static Blog.Application.Features.Posts.Queries.GetPosts;

namespace Blog.UnitTests.Presentation;

public class PostsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly PostsController _controller;

    public PostsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new PostsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task CreatePost_Should_Return_ActionResult()
    {
        var createPostDto = new CreatePostDto("Valid Content", "Valid Title", "Valid FriendlyUrl");
        var expectedResult = Result.Success(Success.Created(new PostDto(1, createPostDto.Title, createPostDto.Content, createPostDto.FriendlyUrl, Guid.NewGuid().ToString(), DateTime.UtcNow)));

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreatePostCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

        var result = await _controller.CreatePost(createPostDto, It.IsAny<CancellationToken>()) as IActionResult;

        Assert.NotNull(result);
        Assert.IsAssignableFrom<CreatedResult>(result);
    }

    [Fact]
    public async Task DeletePost_Should_Return_ActionResult()
    {
        var postId = 123;
        var expectedResult = Result.Success(Success.NoContent());

        _mediatorMock.Setup(m => m.Send(It.IsAny<DeletePostCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

        var result = await _controller.DeletePost(postId, It.IsAny<CancellationToken>()) as IActionResult;

        Assert.NotNull(result);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdatePost_Should_Return_ActionResult()
    {
        var updatePostDto = new UpdatePostDto(1, "Valid Title", "Valid Content", "Valid FriendlyUrl");
        var cancellationToken = CancellationToken.None;
        var expectedResult = Result.Success(Success.Ok(new PostDto(1, updatePostDto.Title, updatePostDto.Content, updatePostDto.FriendlyUrl, Guid.NewGuid().ToString(), DateTime.UtcNow)));

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdatePostCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

        var result = await _controller.UpdatePost(updatePostDto, It.IsAny<CancellationToken>()) as IActionResult;

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetPosts_Should_Return_ActionResult()
    {
        var cancellationToken = CancellationToken.None;
        var pageIndex = 1;
        var pageSize = 10;
        var expectedResult = Result.Success(Success.Ok(new List<PostDto> { new(1, "Valid Title", "Valid Content", "Valid FriendlyUrl", Guid.NewGuid().ToString(), DateTime.UtcNow) }));
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetPostsQuery>(), cancellationToken)).ReturnsAsync(expectedResult);

        var result = await _controller.GetPosts(cancellationToken, pageIndex, pageSize) as IActionResult;

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task ImportPostsFromExcel_Should_Return_ActionResult()
    {
        var formFileMock = new Mock<IFormFile>();
        var cancellationToken = CancellationToken.None;
        var expectedResult = Result.Success(Success.Accept(string.Empty));

        _mediatorMock.Setup(m => m.Send(It.IsAny<ImportPostsFromExcelCommand>(), cancellationToken)).ReturnsAsync(expectedResult);

        var result = await _controller.ImportPostsFromExcel(formFileMock.Object, cancellationToken) as IActionResult;

        Assert.NotNull(result);
        Assert.IsType<AcceptedResult>(result);
    }
}
