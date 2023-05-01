using ApiBenchmark.Services.Models;
using Refit;

namespace ApiBenchmark.Services.Clients.Interfaces;

public interface IRefitClient
{
    [Get("/api/live?")]
    Task<IApiResponse<ForexResponse>> GetRates(string pairs);
}