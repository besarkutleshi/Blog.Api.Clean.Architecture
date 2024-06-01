using Blog.SharedResources;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Blog.Presentation.Helpers;

public static class ActionResponse
{
    public static IActionResult Result(Result result)
    {
        HttpStatusCode httpStatusCode;

        if (result.IsSuccess)
        {
            httpStatusCode = result.Response.GetHttpStatusCodeBySuccessType();

            return GetActionResult(httpStatusCode, result);
        }

        httpStatusCode = result.Error.GetHttpStatusCodeByErrorType();

        return GetActionResult(httpStatusCode, result);
    }

    private static IActionResult GetActionResult(HttpStatusCode httpStatusCode, Result result)
    {
        return httpStatusCode switch
        {
            HttpStatusCode.OK => new OkObjectResult(result),
            HttpStatusCode.NoContent => new NoContentResult(),
            HttpStatusCode.Created => new CreatedResult("resurceUri", result),
            HttpStatusCode.Accepted => new AcceptedResult("resurceUri", result),
            HttpStatusCode.BadRequest => new BadRequestObjectResult(result),
            HttpStatusCode.Forbidden => new UnauthorizedObjectResult(result),
            HttpStatusCode.NotFound => new NotFoundObjectResult(result),
            HttpStatusCode.Unauthorized => new UnauthorizedObjectResult(result),
            HttpStatusCode.Conflict => new ConflictObjectResult(result),
            HttpStatusCode.InternalServerError => new ObjectResult(result) { StatusCode = (int)HttpStatusCode.InternalServerError },
            _ => new BadRequestObjectResult(result),
        };
    }
}
