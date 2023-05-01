using System;
using ApiBenchmark.Application.Common.Interfaces;
using ApiBenchmark.Services.Clients;
using ApiBenchmark.Services.Clients.Interfaces;
using ApiBenchmark.Services.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace ApiBenchmark.Services.Module;

public static class ConfigureServices
{
    public static IServiceCollection AddServicesToServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IForexAPIHttpClient, HttpClientService>().ConfigureHttpClient((serviceProvider, httpClient) =>
        {
            httpClient.BaseAddress = new Uri(configuration.GetSection(Constants.Keys.UrlKey).Value);
        });
        services.AddRefitClient<IRefitClient>().ConfigureHttpClient((serviceProvider, httpClient) =>
        {
            httpClient.BaseAddress = new Uri(configuration.GetSection(Constants.Keys.UrlKey).Value);
        });
        
        services.AddSingleton<IRestsharpClient, RestsharpClient>();
        services.AddTransient<IForexAPIRefit, RefitService>();
        services.AddSingleton<IForexAPIRestsharp, RestsharpService>();

        return services;
    }
}