using Blog.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Blog.Infrastructure.Extensions;

public static partial class ApplicationBuilderExtensions
{
    private static ILogger _logger = null!;
    private static IServiceScopeFactory _serviceScopeFactory = null!;

    private static async Task<IApplicationBuilder> RunDbMigration(this IApplicationBuilder app)
    {
        try
        {
            _serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            _logger = app.ApplicationServices.GetRequiredService<ILogger<ApplicationDbContext>>();

            await MigrateAsync(_serviceScopeFactory);

            return app;
        }
        //catch (SqlException ex)
        //{
        //    var handler = ExceptionHandlerUtility.FindExceptionHandler(ex);
        //    if (handler != null)
        //    {
        //        var response = await handler.HandleException(ex, null);
        //        if (response.IsSuccessful)
        //            await MigrateAsync(_serviceScopeFactory);
        //    }

        //    return app;
        //}
        catch (Exception)
        {
            _logger.LogError("Application could not connect to database");

            return app;
        }
    }

    private static async Task MigrateAsync(IServiceScopeFactory serviceScopeFactory)
    {
        try
        {
            var serviceScope = serviceScopeFactory.CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

            if (context != null)
                await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("ERROR: Message: {ex.Message}, Stack: {ex.StackTrace}", ex.Message, ex.StackTrace);

            throw;
        }
    }
}
