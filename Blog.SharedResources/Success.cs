using System.Net;

namespace Blog.SharedResources;

public record Success
{
    public Success(object? response, SuccessType type)
    {
        Result = response;
        Type = type;
    }

    public Success(SuccessType type)
    {
        Type = type;
    }

    public object? Result { get; }
    public SuccessType Type { get; }

    public static Success Ok(object? response) => new(response, SuccessType.Success);
    public static Success Ok() => new(SuccessType.Success);
    public static Success Accept(string message) => new(message, SuccessType.Accepted);
    public static Success Created(object? response) => new(response, SuccessType.Created);
    public static Success NoContent() => new(SuccessType.NoContent);

    public HttpStatusCode GetHttpStatusCodeBySuccessType()
    {
        return Type switch
        {
            SuccessType.Success => HttpStatusCode.OK,
            SuccessType.Created => HttpStatusCode.Created,
            SuccessType.NoContent => HttpStatusCode.NoContent,
            SuccessType.Accepted => HttpStatusCode.Accepted,
            _ => HttpStatusCode.OK,
        };
    }
}
