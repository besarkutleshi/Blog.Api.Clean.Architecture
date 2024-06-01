using Blog.Application.Features.Posts.Dtos.Responses;
using Blog.Domain.Interfaces;
using Blog.SharedResources;
using MapsterMapper;
using MediatR;

namespace Blog.Application.Features.Posts.Queries;

public class GetPosts
{
    public record GetPostsQuery(int PageIndex, int PageSize) : IRequest<Result>;

    public class GetPostsQueryHandler(IPostRepository _postRepository, IMapper _mapper) : IRequestHandler<GetPostsQuery, Result>
    {
        public async Task<Result> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var result = await _postRepository.GetPosts(request.PageIndex, request.PageSize, cancellationToken);
            if (result.IsSuccess)
            {
                var posts = _mapper.Map<List<PostDto>>(result.Response.Result!);
                return Result.Success(Success.Ok(posts));
            }

            return result;
        }
    }
}
