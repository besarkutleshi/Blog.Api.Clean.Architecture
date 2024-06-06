using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure.Extensions;

public static partial class ServiceCollectionExtensions
{
    private static readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    private static IServiceCollection AddCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(MyAllowSpecificOrigins, builder =>
            {
                builder.WithOrigins("*")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin();
            });
        });

        return services;
    }
}
