using ApiBenchmark.Application.Rates;
using BenchmarkDotNet.Attributes;
using Bogus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ApiBenchmark.BenchmarkTests.RestsharpClient;

[MemoryDiagnoser]
public class RestsharpClientBenchmark
{
    private AddRateRestsharpCommand? _command;
    private static readonly decimal Amount = new Random().Next(1, 1000);
    private IMediator? _mediator;
    
    [GlobalSetup]
    public void SetUp()
    {
        _command = new Faker<AddRateRestsharpCommand>()
            .RuleFor(x => x.Amount, Amount)
            .RuleFor(x => x!.SourceCurrency, TestDataConstants.SourceCurrency)
            .RuleFor(x => x!.TargetCurrency, TestDataConstants.TargetCurrency)
            .Generate();

        var services = RestsharpClientServiceConfiguration.RegisterServices();
        
        var provider = services.BuildServiceProvider();
        _mediator = provider.GetRequiredService<IMediator>();
    }
    
    [Benchmark]
    public void BenchMark_RestsharpClientHandler()
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