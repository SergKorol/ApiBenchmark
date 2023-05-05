namespace ApiBenchmark.Services.Models;

public class ForexResponse
{
    public Dictionary<string, Rate>? rates { get; set; }
    public int code { get; set; }
}