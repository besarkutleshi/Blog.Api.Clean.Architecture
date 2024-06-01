namespace Blog.Application.Helpers;

public class RequestService : IRequestService
{
    public string UserId { get; private set; } = null!;

    public void SetUserId(string userId)
    {
        UserId = userId;
    }
}
