namespace Blog.Presentation.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationServiceCollectionConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        AddJwtBearer(services, configuration);
        AddMiddlewares(services);
        AddCors(services);
        services.AddHealthChecks();

        return services;
    }
}
