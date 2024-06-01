using Blog.Presentation.Middlewares;

namespace Blog.Presentation.Extensions;

public static partial class ServiceCollectionExtensions
{
    private static IServiceCollection AddMiddlewares(this IServiceCollection services)
    {
        services.AddTransient<ServiceMiddleware>();

        return services;
    }
}
