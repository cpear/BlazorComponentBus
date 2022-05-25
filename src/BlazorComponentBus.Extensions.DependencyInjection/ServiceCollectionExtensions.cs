using Microsoft.Extensions.DependencyInjection;

namespace BlazorComponentBus.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddBlazorComponentBus(this IServiceCollection services)
    {
        services.AddScoped<IComponentBus, ComponentBus>();
        services.AddScoped<ComponentBus>();
        return services;
    }

}
