using Blog.Presentation.Middlewares;

namespace Blog.Presentation.Extensions;

public static partial class ApplicationBuilderExtensions
{
    private static IApplicationBuilder UseServiceMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ServiceMiddleware>();
        app.UseMiddleware<RequestLogContextMiddleware>();

        return app;
    }
}
