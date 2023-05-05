using ApiBenchmark.Application.Rates;
using BenchmarkDotNet.Attributes;
using Bogus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ApiBenchmark.BenchmarkTests.RefitClient;

[MemoryDiagnoser]
public class RefitClientBenchmark
{
    private AddRateRefitCommand? _command;
    private static readonly decimal Amount = new Random().Next(1, 1000);
    private IMediator? _mediator;
     
    [GlobalSetup]
    public void SetUp()
    {
        _command = new Faker<AddRateRefitCommand>()
            .RuleFor(x => x.Amount, Amount)
            .RuleFor(x => x!.SourceCurrency, TestDataConstants.SourceCurrency)
            .RuleFor(x => x!.TargetCurrency, TestDataConstants.TargetCurrency)
            .Generate();

        var services = RefitClientServiceConfiguration.RegisterServices();
        var provider = services.BuildServiceProvider();
        _mediator = provider.GetRequiredService<IMediator>();
    }
    
    [Benchmark]
    public void BenchMark_RefitClientHandler()
    {
        Handler().ConfigureAwait(false);
    }

    private async Task Handler()
    {
        try
        {
            if (_command != null) await _mediator?.Send(_command, CancellationToken.None)!;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}