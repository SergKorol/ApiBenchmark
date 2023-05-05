using ApiBenchmark.Application.Common.Interfaces;
using ApiBenchmark.Services.Clients.Interfaces;

namespace ApiBenchmark.Services.Clients;

public class RefitService : IForexApiRefit
{
    private readonly IRefitClient _refitClient;
    
    public RefitService(IRefitClient  refitClient)
    {
        _refitClient = refitClient;
    }
    
    
    public async Task<decimal> GetRates(string? sourceCurrency, string? targetCurrency)
    {
        try
        {
            var response = await _refitClient.GetRates($"{sourceCurrency}{targetCurrency}");
            if (!response.IsSuccessStatusCode) throw new Exception("Currency hasn't been found");
            if (response.Content == null) throw new Exception("Currency hasn't been found");
            if (response.Content.rates != null)
                return response.Content.rates.Where(x => x.Key == $"{sourceCurrency}{targetCurrency}")
                    .Select(x => x.Value.rate).FirstOrDefault();
            throw new Exception("Currency hasn't been found");
                      
        } 
        catch (Exception ex)
        {
            Console.WriteLine($"{nameof(RefitService)} : {ex.Message} / {ex.InnerException?.Message} / {ex.InnerException?.StackTrace}");
            throw;
        }
    }
}