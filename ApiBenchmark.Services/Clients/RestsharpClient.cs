using ApiBenchmark.Services.Clients.Interfaces;
using ApiBenchmark.Services.Options;
using Microsoft.Extensions.Options;
using RestSharp;

namespace ApiBenchmark.Services.Clients;

public class RestsharpClient : IRestsharpClient
{
    private readonly RestClient _restClient;

    public RestsharpClient(IOptions<ApiOptions> options)
    {
        var rest = new RestClientOptions(options.Value.ForexUrl);
        _restClient = new RestClient(rest, useClientFactory : true);
    }

    public async Task<RestResponse<T>> ExecuteAsync<T>(RestRequest restRequest)
    {
        var result = await _restClient.ExecuteAsync<T>(restRequest);
        return result;
    }
}