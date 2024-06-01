using Blog.Application.Helpers;
using FluentAssertions;
using Xunit;

namespace Blog.UnitTests.Applications.Helpers;

public class RequestServiceTests
{
    private readonly RequestService _requestService;

    public RequestServiceTests()
    {
        _requestService = new RequestService();
    }

    [Fact]
    public void UserId_Should_Be_Null_By_Default()
    {
        _requestService.UserId.Should().BeNull();
    }

    [Fact]
    public void SetUserId_Should_Set_UserId()
    {
        var expectedUserId = "user-id";

        _requestService.SetUserId(expectedUserId);

        _requestService.UserId.Should().Be(expectedUserId);
    }

    [Fact]
    public void SetUserId_Should_Overwrite_Previous_UserId()
    {
        var initialUserId = "user-id";
        var newUserId = "new-user-id";

        _requestService.SetUserId(initialUserId);
        _requestService.UserId.Should().Be(initialUserId);
        _requestService.SetUserId(newUserId);

        _requestService.UserId.Should().Be(newUserId);
    }
}