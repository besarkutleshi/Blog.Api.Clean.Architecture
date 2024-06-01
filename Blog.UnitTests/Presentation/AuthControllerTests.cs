using Blog.Application.Features.Auth.Dtos.Requests;
using Blog.Presentation.Controllers;
using Blog.SharedResources;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static Blog.Application.Features.Authentication.Commands.Login;

namespace Blog.UnitTests.Presentation;

public class AuthControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new AuthController(_mediatorMock.Object);
    }

    [Fact]
    public async Task Login_Should_Return_ActionResult()
    {
        var loginDto = new LoginDto("email@email.com", "password");
        _mediatorMock.Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(Success.Ok(loginDto)));

        var result = await _controller.Login(loginDto, It.IsAny<CancellationToken>()) as IActionResult;

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }
}
