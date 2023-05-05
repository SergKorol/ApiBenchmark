namespace ApiBenchmark.Application.Common.Interfaces;

public interface IForexApiHttpClient
{
    public Task<decimal> GetRates(string? sourceCurrency, string? targetCurrency);
}