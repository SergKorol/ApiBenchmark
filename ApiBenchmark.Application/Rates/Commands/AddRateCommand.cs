using ApiBenchmark.Application.Enities;
using MediatR;

namespace ApiBenchmark.Application.Rates.Commands;

public record AddRateCommand : IRequest<ForexRate>
{
    public decimal Amount { get; init; }
    public string SourceCurrency { get; init; }
    public string TargetCurrency { get; init; }
}