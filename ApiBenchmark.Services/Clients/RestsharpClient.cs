using ApiBenchmark.Services.Clients.Interfaces;
using ApiBenchmark.Services.Options;
using Microsoft.Extensions.Options;
using RestSharp;

namespace ApiBenchmark.Services.Clients;

public class RestsharpClient : IRestsharpClient
{
    private readonly RestClient? _restClient;

    public RestsharpClient(IOptions<ApiOptions> options)
    {
        _restClient = new RestClient("https://www.freeforexapi.com");
    }

    public async Task<RestResponse<T>> ExecuteAsync<T>(RestRequest restRequest)
    {
        if (_restClient != null)
        {
            var result = await _restClient.ExecuteAsync<T>(restRequest);
            return result;
        }

        return null!;
    }
}