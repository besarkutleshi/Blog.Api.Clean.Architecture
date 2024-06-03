using Blog.Infrastructure.Persistence;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure.Extensions;

public static partial class ServiceCollectionExtensions
{
    private static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        var dbOptions = new DbConnectionStringOptions();
        configuration.GetSection("DbConnectionString").Bind(dbOptions);

        var connectionString = $"Server={dbOptions.Server};Database={dbOptions.Database};User Id={dbOptions.Username};Password={dbOptions.Password};TrustServerCertificate=True";

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

        services.AddHangfireServer();

        return services;
    }
}
