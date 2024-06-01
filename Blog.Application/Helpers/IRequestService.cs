namespace Blog.Application.Helpers;

public interface IRequestService
{
    public string UserId { get; }
    void SetUserId(string userId);
}
