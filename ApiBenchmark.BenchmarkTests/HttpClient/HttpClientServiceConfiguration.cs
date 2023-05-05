using System.Reflection;
using ApiBenchmark.Application.Common.Interfaces;
using ApiBenchmark.Application.Entities;
using ApiBenchmark.Application.Rates;
using ApiBenchmark.Services.Clients;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ApiBenchmark.BenchmarkTests.HttpClient;

public static class HttpClientServiceConfiguration
{
    public static IServiceCollection RegisterServices()
    {
        var services = new ServiceCollection();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddTransient<IForexApiHttpClient, HttpClientService>();
        services.AddTransient<IRequestHandler<AddRateHttpClientCommand, ForexRate>, AddRateHttpClientCommandHandler>();
        services.AddHttpClient<IForexApiHttpClient, HttpClientService>().ConfigureHttpClient(client =>
        {
            client.BaseAddress = new Uri($"{TestDataConstants.BaseApiUrl}/api/live");
            client.Timeout = TimeSpan.FromSeconds(30);
        });
        
        return services;
    }
}