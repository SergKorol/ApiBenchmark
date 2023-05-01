using System.Threading;
using System.Threading.Tasks;
using ApiBenchmark.Application.Common.Interfaces;
using ApiBenchmark.Application.Enities;
using ApiBenchmark.Application.Rates.Commands;
using MediatR;

namespace ApiBenchmark.Application.Rates;


public record AddRateHttpClientCommand : AddRateCommand { }
public class AddRateHttpClientCommandHandler : IRequestHandler<AddRateHttpClientCommand, ForexRate>
{

    private readonly IForexAPIHttpClient _forexHttpClient;
        
    public AddRateHttpClientCommandHandler(IForexAPIHttpClient forexHttpClient)
    {
        _forexHttpClient = forexHttpClient;
    }
        
    public async Task<ForexRate> Handle(AddRateHttpClientCommand request, CancellationToken cancellationToken)
    {
        decimal rate = 1;
        if (request.SourceCurrency != request.TargetCurrency)
        {
            rate = await _forexHttpClient.GetRates(request.SourceCurrency, request.TargetCurrency);
        }
        
        return new ForexRate
        {
            Amount = request.Amount,
            SourceCurrency = request.SourceCurrency,
            TargetCurrency = request.TargetCurrency,
            RateAmount = rate * request.Amount
        };
    }
}