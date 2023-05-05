namespace ApiBenchmark.Application.Entities;

public class ForexRate
{
    public TimeSpan ElapsedTime { get; set; }
    public decimal Amount { get; set; }
    public string? SourceCurrency { get; set; }
    public string? TargetCurrency { get; set; }
    public decimal RateAmount { get; set; }
    public DateTime RequestDate { get; set; }

    public ForexRate()
    {
        RequestDate = DateTime.UtcNow;
    }

    public decimal GetAmount()
    {
        return Math.Round(Amount * RateAmount, 2);
    }
}