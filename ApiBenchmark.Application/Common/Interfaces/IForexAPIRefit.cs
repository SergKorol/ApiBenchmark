namespace ApiBenchmark.Application.Common.Interfaces;

public interface IForexApiRefit
{
    public Task<decimal> GetRates(string? currency, string? targetCurrency);
}