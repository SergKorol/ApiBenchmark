using ApiBenchmark.Application.Common.Interfaces;
using ApiBenchmark.Services.Clients;
using ApiBenchmark.Services.Clients.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace ApiBenchmark.Services.Module;

public static class ConfigureServices
{
    public static IServiceCollection AddServicesToServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IForexApiHttpClient, HttpClientService>().ConfigureHttpClient((serviceProvider, httpClient) =>
        {
            var uriString = configuration.GetSection(Constants.Keys.UrlKey).Value;
            if (uriString != null)
                httpClient.BaseAddress = new Uri(uriString);
        });
        services.AddRefitClient<IRefitClient>().ConfigureHttpClient((serviceProvider, httpClient) =>
        {
            var uriString = configuration.GetSection(Constants.Keys.UrlKey).Value;
            if (uriString != null)
                httpClient.BaseAddress = new Uri(uriString);
        });
        
        services.AddSingleton<IRestsharpClient, RestsharpClient>();
        services.AddTransient<IForexApiRefit, RefitService>();
        services.AddSingleton<IForexApiRestsharp, RestsharpService>();

        return services;
    }
}