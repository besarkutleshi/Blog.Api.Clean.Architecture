using Blog.Application.Features.Posts.Dtos.Requests;
using Blog.Presentation.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Blog.Application.Features.Posts.Commands.CreatePost;
using static Blog.Application.Features.Posts.Commands.DeletePost;
using static Blog.Application.Features.Posts.Commands.ImportPostsFromExcel;
using static Blog.Application.Features.Posts.Commands.UpdatePost;
using static Blog.Application.Features.Posts.Queries.GetPosts;

namespace Blog.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class PostsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreatePost(CreatePostDto createPostDto, CancellationToken cancellationToken)
    {
        var command = new CreatePostCommand(createPostDto);
        var response = await _mediator.Send(command, cancellationToken);

        return ActionResponse.Result(response); 
    }

    [HttpDelete("{postId}")]
    public async Task<IActionResult> DeletePost(int postId, CancellationToken cancellationToken)
    {
        var command = new DeletePostCommand(postId);
        var response = await _mediator.Send(command, cancellationToken);

        return ActionResponse.Result(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePost(UpdatePostDto updatePostDto, CancellationToken cancellationToken)
    {
        var command = new UpdatePostCommand(updatePostDto);
        var response = await _mediator.Send(command, cancellationToken);

        return ActionResponse.Result(response);
    }

    [Authorize(Roles = "Admin, Public")]
    [HttpGet]
    public async Task<IActionResult> GetPosts(CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 10)
    {
        var query = new GetPostsQuery(pageIndex, pageSize);
        var response = await _mediator.Send(query, cancellationToken);

        return ActionResponse.Result(response);
    }

    [HttpPost("import-posts-from-excel")]
    public async Task<IActionResult> ImportPostsFromExcel([FromForm] IFormFile formFile, CancellationToken cancellationToken)
    {
        var command = new ImportPostsFromExcelCommand(formFile);
        var response = await _mediator.Send(command, cancellationToken);

        return ActionResponse.Result(response);
    }
}
