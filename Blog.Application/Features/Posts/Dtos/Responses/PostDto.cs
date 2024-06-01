namespace Blog.Application.Features.Posts.Dtos.Responses;

public record PostDto(int Id, string Title, string Content, string FriendlyUrl, string CreatedBy, DateTime DateCreated);
