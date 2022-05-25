using BlazorComponentBus.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlazorComponentBus.UnitTests;


public sealed class DependencyInjectionTests
{

    [Fact]
    public void ServiceProviderShouldInjectIComponentBus()
    {
        using var serviceProvider = new ServiceCollection().AddBlazorComponentBus().BuildServiceProvider();
        Assert.NotNull(serviceProvider.GetService<IComponentBus>());
    }

    [Fact]
    public void ServiceProviderShouldInjectComponentBus()
    {
        using var serviceProvider = new ServiceCollection().AddBlazorComponentBus().BuildServiceProvider();
        Assert.NotNull(serviceProvider.GetService<ComponentBus>());
    }

}
