using ApiBenchmark.BenchmarkTests.HttpClient;
using ApiBenchmark.BenchmarkTests.RefitClient;
using ApiBenchmark.BenchmarkTests.RestsharpClient;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

[MemoryDiagnoser]
public class Program
{
    static void Main(string[] args)
    {
        var test = string.Join(" ", args);
        Console.WriteLine(test);
        if (args == null || args.Length == 0)
        {
            throw new ArgumentNullException(nameof(args));
        }
        ClientHandler(args);
    }
    
    private static void ClientHandler(string[] args)
    {
        string path;
        string currentDirectory = Directory.GetCurrentDirectory();
        Console.WriteLine($"Current: {currentDirectory}");
        if (currentDirectory != "/app")
        {
            string? parentDirectory = Path.GetDirectoryName(Path.GetDirectoryName(currentDirectory));
            Console.WriteLine($"Parent: {parentDirectory}");
            path = $"{parentDirectory}/ApiBenchmark/ApiBenchmark.BenchmarkTests";
        }
        else
        {
            path = "/app/src/ApiBenchmark/ApiBenchmark.BenchmarkTests";
        }
        
        foreach (var arg in args)
        {
            switch (arg)
            {
                case "HttpClient":
                    var httpClientConfig =
                        DefaultConfig
                            .Instance
                            .WithArtifactsPath($"{path}/Reports/{arg}");
                    RuntimesHandler(httpClientConfig, args);     
                    BenchmarkRunner.Run<HttpClientBenchmark>(httpClientConfig);
                    break;
                case "Refit":
                    var refitClientConfig =
                        DefaultConfig
                            .Instance
                            .WithArtifactsPath($"{path}/Reports/{arg}");
                    RuntimesHandler(refitClientConfig, args);     
                    BenchmarkRunner.Run<RefitClientBenchmark>(refitClientConfig);
                    break;
                case "RestSharp":
                    var restsharpClientConfig =
                        DefaultConfig
                            .Instance
                            .WithArtifactsPath($"{path}/Reports/{arg}");
                    RuntimesHandler(restsharpClientConfig, args);     
                    BenchmarkRunner.Run<RestsharpClientBenchmark>(restsharpClientConfig);
                    break;
            }
        }
    }

    private static void RuntimesHandler(ManualConfig? config, string[] args)
    {
        foreach (var arg in args)
        {
            if (arg != "--runtimes") continue;
            var next = Array.IndexOf(args, arg) + 1;
            while (args[next] != "--filter")
            {
                if (config != null) SetRuntimes(args[next], config);
                next++;
            }
            break;
        }
    }

    private static void SetRuntimes(string runtime, ManualConfig config)
    {
        switch (runtime)
        {
            case "net6.0":
                config.AddJob(Job.ShortRun
                    .WithRuntime(CoreRuntime.Core60));
                break;
            case "net7.0":
                config.AddJob(Job.ShortRun
                    .WithRuntime(CoreRuntime.Core70));
                break;
            case "net8.0":
                config.AddJob(Job.ShortRun
                    .WithRuntime(CoreRuntime.Core80));
                break;
        }
    }
}


