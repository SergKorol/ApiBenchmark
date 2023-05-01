
using ApiBenchmark.Application.Common.Interfaces;
using ApiBenchmark.Application.Enities;
using ApiBenchmark.Application.Rates.Commands;
using MediatR;

namespace ApiBenchmark.App.Rates;

public record AddRateRestsharpCommand : AddRateCommand {}

public class AddRateRestsharpCommandHandler : IRequestHandler<AddRateRestsharpCommand, ForexRate>
{
    private readonly IForexAPIRestsharp _forexRestsharp;

    public AddRateRestsharpCommandHandler(IForexAPIRestsharp forexRestsharp)
    {
        _forexRestsharp = forexRestsharp;
    }

    public async Task<ForexRate> Handle(AddRateRestsharpCommand request, CancellationToken cancellationToken)
    {
        decimal rate = 1;
        if (request.SourceCurrency != request.TargetCurrency)
        {
            rate = await _forexRestsharp.GetRates(request.SourceCurrency, request.TargetCurrency);
        }

        return new ForexRate
        {
            Amount = request.Amount,
            SourceCurrency = request.SourceCurrency,
            TargetCurrency = request.TargetCurrency,
            RateAmount = rate
        };
    }
}