using Blog.Application.Features.Posts.Dtos.Requests;
using Blog.Application.Features.Posts.Dtos.Responses;
using Blog.Domain.Entities;
using Mapster;

namespace Blog.Application.Mappings;

public static class MapsterConfigs
{
    public static void Configure()
    {
        TypeAdapterConfig<CreatePostDto, Post>.NewConfig();
        TypeAdapterConfig<UpdatePostDto, Post>.NewConfig();
        TypeAdapterConfig<Post, PostDto>.NewConfig();
    }
}
