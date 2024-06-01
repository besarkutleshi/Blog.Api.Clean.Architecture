using Microsoft.AspNetCore.Identity;

namespace Blog.Domain.Entities;
public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? FriendlyUrl { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }

    public virtual IdentityUser? CreatedByUser { get; set; }
}