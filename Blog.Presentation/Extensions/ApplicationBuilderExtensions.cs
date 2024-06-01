namespace Blog.Presentation.Extensions;

public static partial class ApplicationBuilderExtensions
{
    public static IApplicationBuilder AddPersistentApplicationBuilderConfigurations(this IApplicationBuilder app)
    {
        UseServiceMiddleware(app);

        return app;
    }
}
