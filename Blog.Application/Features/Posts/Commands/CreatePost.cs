using Blog.Application.Features.Posts.Dtos.Requests;
using Blog.Application.Features.Posts.Dtos.Responses;
using Blog.Application.Helpers;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Blog.SharedResources;
using MapsterMapper;
using MediatR;

namespace Blog.Application.Features.Posts.Commands;

public class CreatePost
{
    public record CreatePostCommand(CreatePostDto CreatePostDto) : IRequest<Result>;

    public class CreatePostCommandHandler(IPostRepository _postRepository, IMapper _mapper, IRequestService _requestService) : IRequestHandler<CreatePostCommand, Result>
    {
        public async Task<Result> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var postEntity = _mapper.Map<Post>(request.CreatePostDto);
            postEntity.CreatedBy = _requestService.UserId;

            var result = await _postRepository.CreatePost(postEntity, cancellationToken);
            if (result.IsSuccess)
            {
                var postDto = _mapper.Map<PostDto>(result.Response.Result!);
                return Result.Success(Success.Created(postDto));
            }

            return Result.Failure(result.Error);
        }
    }
}
