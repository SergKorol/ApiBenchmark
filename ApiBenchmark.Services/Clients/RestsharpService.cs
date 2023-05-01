using ApiBenchmark.Application.Common.Interfaces;
using ApiBenchmark.Services.Clients.Interfaces;
using ApiBenchmark.Services.Models;
using RestSharp;

namespace ApiBenchmark.Services.Clients;

public class RestsharpService : IForexAPIRestsharp
{
    private readonly IRestsharpClient _restsharpClient;
    
    public RestsharpService(IRestsharpClient restsharpClient)
    {
        _restsharpClient = restsharpClient;
    }
    
    public async Task<decimal> GetRates(string sourceCurrency, string targetCurrency)
    {
        try
        {
            RestRequest request = new RestRequest($"api/live?pairs={sourceCurrency}{targetCurrency}");
            var response = await _restsharpClient.ExecuteAsync<ForexResponse>(request);
            if(response.IsSuccessful)
            {
                return response.Data.rates.Where(x => x.Key == $"{sourceCurrency}{targetCurrency}")
                    .Select(x => x.Value.rate).FirstOrDefault();
            }
            throw response.ErrorException;
        } 
        catch (Exception ex)
        {
            Console.WriteLine($"{nameof(RestsharpService)} : {ex.Message} / {ex.InnerException.Message} / {ex.InnerException.StackTrace}");
            throw ex;
        }
    }
}