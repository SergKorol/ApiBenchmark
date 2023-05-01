using ApiBenchmark.Application.Common.Interfaces;
using ApiBenchmark.Services.Clients.Interfaces;
using ApiBenchmark.Services.Models;
using Newtonsoft.Json;

namespace ApiBenchmark.Services.Clients;

public class RefitService : IForexAPIRefit
{
    private readonly IRefitClient _refitClient;
    
    public RefitService(IRefitClient  refitClient)
    {
        _refitClient = refitClient;
    }
    
    
    public async Task<decimal> GetRates(string sourceCurrency, string targetCurrency)
    {
        try
        {
            
            var response = await _refitClient.GetRates($"{sourceCurrency}{targetCurrency}");
            if (response.IsSuccessStatusCode)
                return response.Content.rates.Where(x => x.Key == $"{sourceCurrency}{targetCurrency}")
                    .Select(x => x.Value.rate).FirstOrDefault();
            throw new Exception("Currency hasn't been found");
                      
        } 
        catch (Exception ex)
        {
            Console.WriteLine($"{nameof(RefitService)} : {ex.Message} / {ex.InnerException.Message} / {ex.InnerException.StackTrace}");
            throw ex;
        }
    }
}