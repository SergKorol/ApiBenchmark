using System.Threading.Tasks;

namespace ApiBenchmark.Application.Common.Interfaces;

public interface IForexAPIRefit
{
    public Task<decimal> GetRates(string currency, string targetCurrency);
}