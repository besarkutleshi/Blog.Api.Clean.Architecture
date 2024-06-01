using Blog.Infrastructure.Seeds;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure.Extensions;

public static partial class ApplicationBuilderExtensions
{
    public static async Task<IApplicationBuilder> AddInfrastructureApplicationConfigurations(this IApplicationBuilder app)
    {
        await RunDbMigration(app);

        var scope = app.ApplicationServices.CreateScope();

        //await SeedDb.CreateIdentityInstances(scope);

        return app;
    }
}
