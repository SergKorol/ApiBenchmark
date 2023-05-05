using System.Reflection;
using ApiBenchmark.Application.Common.Interfaces;
using ApiBenchmark.Application.Entities;
using ApiBenchmark.Application.Rates;
using ApiBenchmark.Services.Clients;
using ApiBenchmark.Services.Clients.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ApiBenchmark.BenchmarkTests.RestsharpClient;

public static class RestsharpClientServiceConfiguration
{
    public static IServiceCollection RegisterServices()
    {
        var services = new ServiceCollection();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddTransient<IForexApiRestsharp, RestsharpService>();
        services.AddTransient<IRequestHandler<AddRateRestsharpCommand, ForexRate>, AddRateRestsharpCommandHandler>();
        services.AddSingleton<IRestsharpClient, Services.Clients.RestsharpClient>();
        
        return services;
    }
}