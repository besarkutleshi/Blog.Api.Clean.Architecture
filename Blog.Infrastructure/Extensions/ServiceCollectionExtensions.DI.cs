using Blog.Domain.Interfaces;
using Blog.Infrastructure.BackgroundJobs;
using Blog.Infrastructure.Files;
using Blog.Infrastructure.Imports;
using Blog.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure.Extensions;

public static partial class ServiceCollectionExtensions
{
    private static IServiceCollection AddDI(this IServiceCollection services)
    {
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddTransient<IAuthorizationRepository, AuthorizationRepository>();
        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        services.AddTransient<IImportPostsFromExcel, ImportPostsFromExcel>();
        services.AddTransient<IFileSaver, FileSaver>();
        services.AddScoped<IEnqueueJob, EnqueueJob>();

        return services;
    }
}
