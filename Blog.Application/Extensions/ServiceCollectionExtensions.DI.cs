using Blog.Application.Behaviours;
using Blog.Application.Contracts;
using Blog.Application.Factories;
using Blog.Application.Helpers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Blog.Application.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddDI(this IServiceCollection services)
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };
        jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

        services.AddSingleton(jsonSerializerOptions);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped<IRequestService, RequestService>();
        services.AddScoped<IAuthTokenFactory, AuthTokenFactory>();

        return services;
    }
}
