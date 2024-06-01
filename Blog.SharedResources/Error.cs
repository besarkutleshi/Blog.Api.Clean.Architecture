using System.Net;

namespace Blog.SharedResources;

public record Error
{
    public Error(string code, List<string> errorMessages, ErrorType type)
    {
        Code = code;
        ErrorMessages = errorMessages;
        Type = type;
    }

    public string Code { get; }
    public List<string> ErrorMessages { get; }
    public ErrorType Type { get; set; }

    public static Error NotFound(string code, List<string> errorMessages) 
        => new(code, errorMessages, ErrorType.NotFound);
    public static Error Validation(string code, List<string> errorMessages)
        => new(code, errorMessages, ErrorType.Validation);
    public static Error Conflict(string code, List<string> errorMessages)
        => new(code, errorMessages, ErrorType.Conflict);
    public static Error Failure(string code, List<string> errorMessages)
        => new(code, errorMessages, ErrorType.Failure);

    public HttpStatusCode GetHttpStatusCodeByErrorType()
    {
        return Type switch
        {
            ErrorType.NotFound => HttpStatusCode.NotFound,
            ErrorType.Validation => HttpStatusCode.BadRequest,
            ErrorType.Conflict => HttpStatusCode.Conflict,
            ErrorType.Failure => HttpStatusCode.InternalServerError,
            _ => HttpStatusCode.InternalServerError,
        };
    }
}
