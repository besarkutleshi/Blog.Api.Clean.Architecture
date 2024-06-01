using Blog.Application.Features.Posts.Dtos.Responses;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Blog.SharedResources;
using MapsterMapper;
using Moq;
using Xunit;
using static Blog.Application.Features.Posts.Queries.GetPosts;

namespace Blog.UnitTests.Applications.Features.Posts.Queries;

public class GetPostsTests
{
    private readonly Mock<IPostRepository> _mockPostRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetPostsQueryHandler _getPostsQueryHandler;

    public GetPostsTests()
    {
        _mockPostRepository = new Mock<IPostRepository>();
        _mockMapper = new Mock<IMapper>();
        _getPostsQueryHandler = new(
            _mockPostRepository.Object,
            _mockMapper.Object
        );
    }

    [Fact]
    public async Task Should_Have_Error_When_Repository_Returns_Failure()
    {
        var pageIndex = 1;
        var pageSize = 10;

        _mockPostRepository.Setup(x => x.GetPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(Error.Failure("GetPosts.Error", ["Something went wrong, please try again later"])));

        var query = new GetPostsQuery(pageIndex, pageSize);
        var result = await _getPostsQueryHandler.Handle(query, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task Should_Have_No_Error_When_Repository_Returns_Success()
    {
        var pageIndex = 1;
        var pageSize = 10;

        _mockPostRepository.Setup(x => x.GetPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(Success.Ok(new List<Post>())));

        _mockMapper.Setup(x => x.Map<List<PostDto>>(It.IsAny<List<Post>>()))
            .Returns([]);

        var query = new GetPostsQuery(pageIndex, pageSize);
        var result = await _getPostsQueryHandler.Handle(query, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.NotNull(result.Response);
        Assert.NotNull(result.Response.Result);
        Assert.IsType<List<PostDto>>(result.Response.Result);
    }
}
