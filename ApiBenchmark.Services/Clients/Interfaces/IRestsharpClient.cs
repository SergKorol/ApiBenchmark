using RestSharp;

namespace ApiBenchmark.Services.Clients.Interfaces;

public interface IRestsharpClient
{
    public Task<RestResponse<T>> ExecuteAsync<T>(RestRequest restRequest);
}