using ApiBenchmark.Application.Common.Interfaces;
using ApiBenchmark.Application.Entities;
using ApiBenchmark.Application.Rates.Commands;
using MediatR;

namespace ApiBenchmark.Application.Rates;

public record AddRateRefitCommand : AddRateCommand;

public class AddRateRefitCommandHandler : IRequestHandler<AddRateRefitCommand, ForexRate>
{
    private readonly IForexApiRefit _forexRefit;

    public AddRateRefitCommandHandler(IForexApiRefit forexRefit)
    {
        _forexRefit = forexRefit;
    }

    public async Task<ForexRate> Handle(AddRateRefitCommand request, CancellationToken cancellationToken)
    {
        decimal rate = request.SourceCurrency == request.TargetCurrency
            ? 1
            : await _forexRefit.GetRates(request.SourceCurrency,
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