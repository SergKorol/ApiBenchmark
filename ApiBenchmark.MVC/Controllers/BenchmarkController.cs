using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace ApiBenchmark.MVC.Controllers;

public class BenchmarkController : Controller
{
    public IActionResult BenchmarkReports(string reportType)
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string? parentDirectory = currentDirectory != "/app" ? Path.GetDirectoryName(Path.GetDirectoryName(currentDirectory)) : "/app/src";
        if (parentDirectory != null)
        {
            HtmlDocument doc;
            string html;
            ContentResult contentResult;
            
            if (reportType == "All")
            {
                var baseFolderPath = Path.Combine(parentDirectory, "ApiBenchmark", "ApiBenchmark.BenchmarkTests", "Reports");
                var allFiles = Directory.GetFiles(baseFolderPath, "*.html", SearchOption.AllDirectories);
                List<string> htmls = new List<string>();
                doc = new HtmlDocument();
                foreach (var file in allFiles)
                {
                    doc.Load(file);
                    htmls.Add(doc.DocumentNode.SelectSingleNode("//html").InnerHtml);
                }
                var unitedReport = string.Concat(htmls);
                contentResult = new ContentResult { Content = unitedReport, ContentType = "text/html" };
                return RedirectToAction("HtmlReport", "Rate", contentResult);
            }
            
            
            string path = Path.Combine(parentDirectory, "ApiBenchmark", "ApiBenchmark.BenchmarkTests", "Reports", reportType, "results");
            string? reportPath = Directory.GetFiles(path, "*.html").FirstOrDefault();

            if (reportPath == null)
            {
                return NotFound("No report found.");
            }

            doc = new HtmlDocument();
            doc.Load(reportPath);
            html = doc.DocumentNode.SelectSingleNode("//html").InnerHtml;

            contentResult = new ContentResult { Content = html, ContentType = "text/html" };
            return RedirectToAction("HtmlReport", "Rate", contentResult);
        }
        
        throw  new Exception("Could not find parent directory.");
    }
    public async Task<RedirectToActionResult> BenchmarkRunTests(string host, IEnumerable<string> runtimes, string client)
    {
        var usedFrameworks = runtimes.ToArray();
        await ScriptRunner.ScriptRunner.Run(host, usedFrameworks, client);

        return RedirectToAction("Index", "Rate");
    }
}