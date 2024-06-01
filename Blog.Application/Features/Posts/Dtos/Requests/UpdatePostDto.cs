namespace Blog.Application.Features.Posts.Dtos.Requests;

public record UpdatePostDto(int Id, string Title, string Content, string FriendlyUrl);
