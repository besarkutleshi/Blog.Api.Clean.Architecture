namespace Blog.Application.Features.Posts.Dtos.Requests;

public record CreatePostDto(string Content, string Title, string FriendlyUrl);
