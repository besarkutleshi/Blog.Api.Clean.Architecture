using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServiceCollectionsConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        AddEFCore(services, configuration);
        AddDI(services);
        AddIdentity(services);
        AddHangfire(services, configuration);

        return services;
    }
}
