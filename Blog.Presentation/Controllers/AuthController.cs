using Blog.Application.Features.Auth.Dtos.Requests;
using Blog.Presentation.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Blog.Application.Features.Authentication.Commands.Login;

namespace Blog.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Login(LoginDto loginDto, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(loginDto);
        var result = await _mediator.Send(command, cancellationToken);

        return ActionResponse.Result(result);
    }
}
