using System.Reflection;
using ApiBenchmark.Application.Common.Interfaces;
using ApiBenchmark.Application.Entities;
using ApiBenchmark.Application.Rates;
using ApiBenchmark.Services.Clients;
using ApiBenchmark.Services.Clients.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace ApiBenchmark.BenchmarkTests.RefitClient;

public static class RefitClientServiceConfiguration
{
    public static IServiceCollection RegisterServices()
    {
        var services = new ServiceCollection();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddTransient<IForexApiRefit, RefitService>();
        services.AddTransient<IRequestHandler<AddRateRefitCommand, ForexRate>, AddRateRefitCommandHandler>();
        
        services.AddRefitClient<IRefitClient>().ConfigureHttpClient( httpClient =>
        {
            httpClient.BaseAddress = new Uri(TestDataConstants.BaseApiUrl);
            httpClient.Timeout  = TimeSpan.FromSeconds(10);
        });
        
        return services;
    }
}