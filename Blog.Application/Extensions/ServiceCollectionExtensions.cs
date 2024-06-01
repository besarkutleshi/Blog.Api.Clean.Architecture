using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Application.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServiceCollectionConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        AddFluentValidation(services);
        AddMediator(services);
        AddMapsterConfigurations(services);
        AddDI(services);

        return services;
    }
}
