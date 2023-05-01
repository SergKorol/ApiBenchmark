using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ApiBenchmark.Application;

public static class AppModule
{
    public static IServiceCollection AddProductModule(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return serviceCollection;
    }
}
