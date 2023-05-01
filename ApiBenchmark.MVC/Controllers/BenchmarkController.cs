using System.Diagnostics;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace ApiBenchmark.MVC.Controllers;

public class BenchmarkController : Controller
{
    public IActionResult BenchmarkReports(string reportType)
    {
        
        
        // string path;
        // string currentDirectory = Directory.GetCurrentDirectory();
        // Console.WriteLine($"Current: {currentDirectory}");
        // if (currentDirectory != "/app")
        // {
        //     string? parentDirectory = Path.GetDirectoryName(Path.GetDirectoryName(currentDirectory));
        //     Console.WriteLine($"Parent: {parentDirectory}");
        //     if (reportType == "All")
        //     {
        //         path = $"{parentDirectory}/ApiBenchmark/ApiBenchmark.BenchmarkTests/Reports/results";
        //     }
        //     else
        //     {
        //         path = $"{parentDirectory}/ApiBenchmark/ApiBenchmark.BenchmarkTests/Reports/{reportType}/results";
        //     }
        // }
        // else
        // {
        //     if (reportType == "All")
        //     {
        //         path = $"/app/src/ApiBenchmark/ApiBenchmark.BenchmarkTests/Reports/results";
        //     }
        //     else
        //     {
        //         path = $"/app/src/ApiBenchmark/ApiBenchmark.BenchmarkTests/Reports/{reportType}/results";
        //     }
        // }
        // string? reportPath = Directory.GetFiles(path, "*.html").FirstOrDefault();
        string currentDirectory = Directory.GetCurrentDirectory();
        Console.WriteLine($"Current: {currentDirectory}");
        string parentDirectory = currentDirectory != "/app" ? Path.GetDirectoryName(Path.GetDirectoryName(currentDirectory)) : "/app/src";
        Console.WriteLine($"Parent: {parentDirectory}");
        string path = Path.Combine(parentDirectory, "ApiBenchmark", "ApiBenchmark.BenchmarkTests", "Reports", reportType == "All" ? "" : reportType, "results");
        string? reportPath = Directory.GetFiles(path, "*.html").FirstOrDefault();

        if (reportPath == null)
        {
            return NotFound("No report found.");
        }

        var doc = new HtmlDocument();
        doc.Load(reportPath);
        string html = doc.DocumentNode.SelectSingleNode("//html").InnerHtml;

        var contentResult = new ContentResult { Content = html, ContentType = "text/html" };
        return RedirectToAction("HtmlReport", "Rate", contentResult);
    }
    public async Task<RedirectToActionResult> BenchmarkRunTests(string host, IEnumerable<string> runtimes, string client)
    {
        await ScriptRunner.Run(host, runtimes, client);

        return RedirectToAction("Index", "Rate");
    }
}