using System.Threading.Tasks;

namespace ApiBenchmark.Application.Common.Interfaces;

public interface IForexAPIRestsharp
{
    public Task<decimal> GetRates(string currency, string targetCurrency);
}