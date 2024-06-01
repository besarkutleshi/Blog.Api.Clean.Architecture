using Blog.Application.Helpers;
using Blog.Presentation.Extensions;
using Blog.SharedResources;
using FluentValidation;
using System.Text.Json;

namespace Blog.Presentation.Middlewares;

public class ServiceMiddleware(ILogger<ServiceMiddleware> logger, IRequestService requestService, IServiceScopeFactory serviceScopeFactory) : IMiddleware
{
    private JsonSerializerOptions _jsonSerializerOptions = null!;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var scope = serviceScopeFactory.CreateScope();
        _jsonSerializerOptions = scope.ServiceProvider.GetRequiredService<JsonSerializerOptions>();

        try
        {
            var userId = context.GetUserId();
            requestService.SetUserId(userId);

            await next(context);
        }
        catch (ValidationException ex)
        {
            var result = Result.Failure(Error.Validation("Validation.Error", ex.Errors.Select(x => x.ErrorMessage).ToList()));

            logger.LogError("");

            await context.WriteBody(result.Error.GetHttpStatusCodeByErrorType(), _jsonSerializerOptions, result);
        }
        catch (Exception ex)
        {
            await InternalServerErrorResponse(context);
        }
    }

    private async Task InternalServerErrorResponse(HttpContext context)
    {
        var result = Result.Failure(Error.Failure("Failure", ["Internal Server Error"]));
        await context.WriteBody(result.Error.GetHttpStatusCodeByErrorType(), _jsonSerializerOptions, result);
    }
}
