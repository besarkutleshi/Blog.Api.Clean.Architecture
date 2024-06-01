using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;

namespace Blog.Application.Extensions;

public static partial class ServiceCollectionExtensions
{
    private static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }
}