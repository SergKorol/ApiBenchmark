using ApiBenchmark.Application.Common.Interfaces;
using ApiBenchmark.Application.Entities;
using ApiBenchmark.Application.Rates.Commands;
using MediatR;

namespace ApiBenchmark.Application.Rates;

public record AddRateRestsharpCommand : AddRateCommand;

public class AddRateRestsharpCommandHandler : IRequestHandler<AddRateRestsharpCommand, ForexRate>
{
    private readonly IForexApiRestsharp _forexRestsharp;

    public AddRateRestsharpCommandHandler(IForexApiRestsharp forexRestsharp)
    {
        _forexRestsharp = forexRestsharp;
    }

    public async Task<ForexRate> Handle(AddRateRestsharpCommand request, CancellationToken cancellationToken)
    {
        decimal rate = request.SourceCurrency == request.TargetCurrency
            ? 1
            : await _forexRestsharp.GetRates(request.SourceCurrency,
                request.TargetCurrency);

        return new ForexRate
        {
            Amount = request.Amount,
            SourceCurrency = request.SourceCurrency,
            TargetCurrency = request.TargetCurrency,
            RateAmount = rate * request.Amount
        };
    }
}