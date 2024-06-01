using System.Net;
using System.Text.Json;

namespace Blog.Presentation.Extensions;

public static class WriteBodyHttpContextExtension
{
    public static async Task WriteBody(this HttpContext httpContext, HttpStatusCode httpStatusCode, JsonSerializerOptions jsonOptions, object error)
    {
        var result = JsonSerializer.Serialize(error, jsonOptions);
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)httpStatusCode;
        await httpContext.Response.WriteAsync(result);
    }

    public static string GetUserId(this HttpContext? httpContext)
    {
        if (httpContext == null)
            return string.Empty;

        var claim = httpContext.User.Claims.SingleOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
        var userId = claim == null ? string.Empty : claim.Value.ToString();

        return userId;
    }
}
