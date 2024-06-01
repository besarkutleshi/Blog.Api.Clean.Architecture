using Blog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure.Extensions;

public static partial class ServiceCollectionExtensions
{
    private static IServiceCollection AddEFCore(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("eBlog");

        if (!string.IsNullOrEmpty(connectionString))
        {
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseNpgsql(connectionString, sqlOptions =>
            //    {
            //        sqlOptions.EnableRetryOnFailure(
            //            maxRetryCount: 2,
            //            maxRetryDelay: TimeSpan.FromSeconds(30),
            //            errorCodesToAdd: null!);
            //    }));

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
