using Blog.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Data.Common;

namespace Blog.IntegrationTests;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private IServiceScope _serviceScope = null!;
    private DbConnectionStringOptions _dbOptions = null!;

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var options = serviceProvider.GetService<IOptions<DbConnectionStringOptions>>();
            if(options is not null)
            {
                _dbOptions = options.Value;
                var connectionString = $"Server={_dbOptions.Server};Database={_dbOptions.IntegrationTestDatabase};User Id={_dbOptions.Username};Password={_dbOptions.Password};TrustServerCertificate=True";

                services.Remove(services.SingleOrDefault(service => typeof(DbContextOptions<ApplicationDbContext>) == service.ServiceType)!);
                services.Remove(services.SingleOrDefault(service => typeof(DbConnection) == service.ServiceType)!);

                services.AddDbContext<ApplicationDbContext>((_, option) => option.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null!);
                }));
            }

            _serviceScope = serviceProvider.CreateScope();
        });
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        return Task.CompletedTask;
        //var context = _serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        //await context.Database.EnsureDeletedAsync();
    }
}
