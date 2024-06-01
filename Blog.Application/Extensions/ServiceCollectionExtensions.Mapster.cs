using Blog.Application.Mappings;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Application.Extensions;

public static partial class ServiceCollectionExtensions
{
    private static IServiceCollection AddMapsterConfigurations(this IServiceCollection services)
    {
        services.AddMapster();
        MapsterConfigs.Configure();

        return services;
    }
}
