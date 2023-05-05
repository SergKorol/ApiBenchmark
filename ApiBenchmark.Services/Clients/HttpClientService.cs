using ApiBenchmark.Application.Common.Interfaces;
using ApiBenchmark.Services.Models;
using Newtonsoft.Json;

namespace ApiBenchmark.Services.Clients;

public class HttpClientService : IForexApiHttpClient
{

    private HttpClient _httpClient;
    
    public HttpClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<decimal> GetRates(string? sourceCurrency, string? targetCurrency)
    {
        try
        {
            var uri = $"{_httpClient.BaseAddress?.OriginalString}/api/live?pairs={sourceCurrency}{targetCurrency}";
            string resultJson = await _httpClient.GetStringAsync(new Uri(uri));
            ForexResponse? response = JsonConvert.DeserializeObject<ForexResponse>(resultJson);
            if (response != null && response.code == 200)
                if (response.rates != null)
                    return response.rates.Where(x => x.Key == $"{sourceCurrency}{targetCurrency}")
                        .Select(x => x.Value.rate).FirstOrDefault();
            throw new Exception("Currency hasn't been found");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}