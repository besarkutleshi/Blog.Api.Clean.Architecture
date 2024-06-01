namespace Blog.Application.Features.Posts.Dtos.Requests;

public record GetPosts(int Id, string Title, string Content, string FriendlyUrl, Guid CreatedBy, DateTime DateCreated);
