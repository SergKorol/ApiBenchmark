using ApiBenchmark.Application.Common.Interfaces;
using ApiBenchmark.Application.Enities;
using ApiBenchmark.Application.Rates.Commands;
using MediatR;

namespace ApiBenchmark.App.Rates;

public record AddRateRefitCommand : AddRateCommand {}

public class AddRateRefitCommandHandler : IRequestHandler<AddRateRefitCommand, ForexRate>
{
    private readonly IForexAPIRefit _forexRefit;

    public AddRateRefitCommandHandler(IForexAPIRefit forexRefit)
    {
        _forexRefit = forexRefit;
    }

    public async Task<ForexRate> Handle(AddRateRefitCommand request, CancellationToken cancellationToken)
    {
        decimal rate = 1;
        if (request.SourceCurrency != request.TargetCurrency)
        {
            rate = await _forexRefit.GetRates(request.SourceCurrency, request.TargetCurrency);
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