namespace ApiBenchmark.Application.Common.Interfaces;

public interface IForexApiRestsharp
{
    public Task<decimal> GetRates(string? currency, string? targetCurrency);
}