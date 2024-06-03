using Blog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure.Extensions;

public static partial class ServiceCollectionExtensions
{
    private static IServiceCollection AddEFCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DbConnectionStringOptions>(configuration.GetSection("DbConnectionString"));

        var dbOptions = new DbConnectionStringOptions();
        configuration.GetSection("DbConnectionString").Bind(dbOptions);

        var connectionString = $"Server={dbOptions.Server};Database={dbOptions.Database};User Id={dbOptions.Username};Password={dbOptions.Password};TrustServerCertificate=True";

        if (!string.IsNullOrEmpty(connectionString))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 2,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null!);
                }));
        }

        return services;
    }
}
