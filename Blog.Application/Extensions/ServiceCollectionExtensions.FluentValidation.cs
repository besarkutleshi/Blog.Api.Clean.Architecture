using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Blog.Application.Extensions;

public static partial class ServiceCollectionExtensions
{
    private static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}