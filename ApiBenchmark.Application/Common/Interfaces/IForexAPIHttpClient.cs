using System.Threading.Tasks;

namespace ApiBenchmark.Application.Common.Interfaces;

public interface IForexAPIHttpClient
{
    public Task<decimal> GetRates(string sourceCurrency, string targetCurrency);
}