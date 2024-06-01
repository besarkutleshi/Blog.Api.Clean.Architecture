namespace Blog.SharedResources;

public class Result
{
    public Result()
    {
        
    }
    private Result(bool isSuccess, Success success)
    {
        IsSuccess = isSuccess;
        Response = success;
    }

    private Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Success Response { get; } = null!;
    public Error Error { get; } = null!;

    public static Result Success(Success success) => new(true, success);
    public static Result Failure(Error error) => new(false, error);
}
