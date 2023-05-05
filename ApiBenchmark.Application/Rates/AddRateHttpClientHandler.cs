using ApiBenchmark.Application.Common.Interfaces;
using ApiBenchmark.Application.Entities;
using ApiBenchmark.Application.Rates.Commands;
using MediatR;

namespace ApiBenchmark.Application.Rates
{
    public record AddRateHttpClientCommand : AddRateCommand;

    public class AddRateHttpClientCommandHandler : IRequestHandler<AddRateHttpClientCommand, ForexRate>
    {
        private readonly IForexApiHttpClient _forexHttpClient;

        public AddRateHttpClientCommandHandler(IForexApiHttpClient forexHttpClient)
        {
            _forexHttpClient = forexHttpClient;
        }

        public async Task<ForexRate> Handle(AddRateHttpClientCommand request, CancellationToken cancellationToken)
        {
            decimal rate = request.SourceCurrency == request.TargetCurrency
                ? 1
                : await _forexHttpClient.GetRates(request.SourceCurrency,
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
}