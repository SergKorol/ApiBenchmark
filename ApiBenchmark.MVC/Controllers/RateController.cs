using ApiBenchmark.App.Rates;
using ApiBenchmark.Application.Enities;
using ApiBenchmark.Application.Rates;
using MediatR;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace ApiBenchmark.MVC.Controllers;

public class RateController : Controller
{
    private readonly IMediator _mediator;

    public RateController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public IActionResult Index(ForexRate forexRate)
    {
        if (forexRate.RateAmount == 0)
        {
            return View();
        }
        return View(forexRate);
    }
    
    public IActionResult HtmlReport(ContentResult? html)
    {
        if (html is null)
        {
            return RedirectToAction("Index");
        }

        var model = new Report
        {
            Title = "Report",
            Content = html.Content
        };
        
        return PartialView(model);
    }
    
    
    // [HttpPost("httpclient/rate")]
    public async Task<IActionResult> GetRateHttpClient(string sourceCurrency, string targetCurrency, decimal amount)
    {
        if (sourceCurrency is null || targetCurrency is null || (amount == 0 || amount < 0))
        {
            return RedirectToAction("Index");
        }
        var command = new AddRateHttpClientCommand
        {
            Amount = amount,
            SourceCurrency = sourceCurrency,
            TargetCurrency = targetCurrency
        };
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var response = await _mediator.Send(command);
        watch.Stop();
        response.ElapsedTime = watch.Elapsed;
        return RedirectToAction("Index", response);
    }
    
    // [HttpPost("restsharp/rate")]
    public async Task<IActionResult> GetRateRestSharp(string sourceCurrency, string targetCurrency, decimal amount)
    {
        var command = new AddRateRestsharpCommand
        {
            Amount = amount,
            SourceCurrency = sourceCurrency,
            TargetCurrency = targetCurrency
        };
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var response = await _mediator.Send(command);
        watch.Stop();
        response.ElapsedTime = watch.Elapsed;
        return RedirectToAction("Index", response);
    }
    //
    // [HttpPost("refit/rate")]
    public async Task<IActionResult> GetRateRefit(string sourceCurrency, string targetCurrency, decimal amount)
    {
        var command = new AddRateRefitCommand
        {
            Amount = amount,
            SourceCurrency = sourceCurrency,
            TargetCurrency = targetCurrency
        };
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var response = await _mediator.Send(command);
        watch.Stop();
        response.ElapsedTime = watch.Elapsed;
        return RedirectToAction("Index", response);
    }
    public IActionResult GetRateByType(string sourceCurrency, string targetCurrency, decimal amount, string transportType)
    {
        var tesr = true;
        
        switch (transportType)
        {
            case "HttpClient":
                return RedirectToAction("GetRateHttpClient", new {sourceCurrency, targetCurrency, amount});
            case "Refit":
                return RedirectToAction("GetRateRefit", new {sourceCurrency, targetCurrency, amount});
            case "Restsharp":
                return RedirectToAction("GetRateRestSharp", new {sourceCurrency, targetCurrency, amount});
            default:
                return RedirectToAction("Index");
        }
    }
    
}