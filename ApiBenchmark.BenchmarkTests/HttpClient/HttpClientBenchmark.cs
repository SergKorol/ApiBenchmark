using System.Reflection;
using System.Text;
using ApiBenchmark.Application.Common.Interfaces;
using ApiBenchmark.Application.Enities;
using ApiBenchmark.Application.Rates;
using ApiBenchmark.Application.Rates.Commands;
using ApiBenchmark.Services.Clients;
using ApiBenchmark.Services.Models;
using BenchmarkDotNet.Attributes;
using Bogus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace ApiBenchmark.BenchmarkTests.HttpClient;

[MemoryDiagnoser]
public class HttpClientBenchmark
{
    private AddRateHttpClientCommand _command;
    private const decimal Amount = 10;
    private const string Currency = "EUR";
    private const string TargetCurrency = "GBP";
    
    
    private IMediator _mediator;
    private IForexAPIHttpClient _client;
    private IRequestHandler<AddRateHttpClientCommand, ForexRate> _handler;
     
     [GlobalSetup]
     public void SetUp()
     {
         _command = new Faker<AddRateHttpClientCommand>()
             .RuleFor(x => x.Amount, Amount)
             .RuleFor(x => x.SourceCurrency, Currency)
             .RuleFor(x => x.TargetCurrency, TargetCurrency)
             .Generate();
         
         var services = new ServiceCollection();
         services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
         services.AddTransient<IForexAPIHttpClient, HttpClientService>();
         services.AddTransient<IRequestHandler<AddRateHttpClientCommand, ForexRate>, AddRateHttpClientCommandHandler>();
         services.AddHttpClient<IForexAPIHttpClient, HttpClientService>().ConfigureHttpClient(client =>
         {
                 client.BaseAddress = new Uri("https://www.freeforexapi.com/api/live");
         });
         var provider = services.BuildServiceProvider();
         _mediator = provider.GetRequiredService<IMediator>();
     }
     
     [Benchmark]
     public void BenchMark_HttpClientHandler()
     {
         Handler();
     }

     private async Task Handler()
     {
         try
         {
             await _mediator.Send(_command, CancellationToken.None);
         }
         catch (Exception e)
         {
             Console.WriteLine(e);
             throw;
         }
     }
}

