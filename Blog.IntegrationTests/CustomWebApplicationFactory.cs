using Blog.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;

namespace Blog.IntegrationTests;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    //private PostgreSqlContainer _container = new PostgreSqlBuilder()
    //    .WithImage("postgres:latest")
    //    .WithDatabase("eblog")
    //    .WithUsername("postgres")
    //    .WithPassword("postgres")
    //    .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("pg_isready"))
    //    .WithCleanUp(true)
    //    .Build();

    private IServiceScope _serviceScope;
    private readonly string _connectionString = "Server=BesarKutleshi;Initial Catalog=Blog_Tests;Integrated Security=True;TrustServerCertificate=True";

    public Task InitializeAsync()
    {
        //DockerComposeService.RunDockerComposeUp();

        return Task.CompletedTask;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.Remove(services.SingleOrDefault(service => typeof(DbContextOptions<ApplicationDbContext>) == service.ServiceType)!);
            services.Remove(services.SingleOrDefault(service => typeof(DbConnection) == service.ServiceType)!);

            services.AddDbContext<ApplicationDbContext>((_, option) => option.UseSqlServer(_connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null!);
            }));

            using var scope = services.BuildServiceProvider().CreateScope();
            var serviceProvider = scope.ServiceProvider;

            _serviceScope = serviceProvider.CreateScope();
        });
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        //var context = _serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        //await context.Database.EnsureDeletedAsync();

        return Task.CompletedTask;
    }
}
