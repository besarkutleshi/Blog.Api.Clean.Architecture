using Blog.Application.Features.Posts.Dtos.Requests;
using Blog.Application.Features.Posts.Dtos.Responses;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Blog.SharedResources;
using MapsterMapper;
using MediatR;

namespace Blog.Application.Features.Posts.Commands;

public class UpdatePost
{
    public record UpdatePostCommand(UpdatePostDto UpdatePostDto) : IRequest<Result>;

    public class UpdatePostCommandHandler(IPostRepository _postRepository, IMapper _mapper) : IRequestHandler<UpdatePostCommand, Result>
    {
        public async Task<Result> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var postEntity = _mapper.Map<Post>(request.UpdatePostDto);

            var result = await _postRepository.UpdatePost(postEntity, cancellationToken);
            if (result.IsSuccess)
            {
                var post = _mapper.Map<PostDto>(result.Response.Result!);
                return Result.Success(Success.Ok(post));
            }

            return result;
        }
    }
}
