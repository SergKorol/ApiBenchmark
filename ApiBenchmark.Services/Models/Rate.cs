using Newtonsoft.Json;

namespace ApiBenchmark.Services.Models;

public class Rate
{
    public decimal rate { get; set; }
    public long timestamp { get; set; }
}