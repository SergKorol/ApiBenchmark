// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using System.Reflection;
using System.Text;
using ApiBenchmark.BenchmarkTests;
using ApiBenchmark.BenchmarkTests.HttpClient;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;
using BenchmarkDotNet.Toolchains.DotNetCli;

[MemoryDiagnoser]
public class Program
{
    static void Main(string[] args)
    {
        var test = string.Join(" ", args);
        Console.WriteLine(test);
        if (args == null || args.Length == 0)
        {
            throw new ArgumentNullException("The arguments can't be NULL or empty");
        }
        ClientHandler(args);
    }
    
    private static void ClientHandler(string[] args)
    {
        // BenchmarkRunner.Run<HttpClientBenchmark>(ManualConfig
        //     .Create(DefaultConfig.Instance)
        //     .AddJob(Job.ShortRun));
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
                    var config =
                        DefaultConfig
                            .Instance
                            .WithArtifactsPath($"{path}/Reports/{arg}");
                    RuntimesHandler(config, args);     
                    BenchmarkRunner.Run<HttpClientBenchmark>(config);
                    break;
                case "Refit":
                    // BenchmarkRunner.Run<RefitBenchmark>(ManualConfig.Create(DefaultConfig.Instance.WithArtifactsPath($"{projectDirectory}/{assemblyName}/Reports/{arg}")).AddJob(Job.ShortRun));
                    break;
                case "RestSharp":
                    // BenchmarkRunner.Run<RestSharpBenchmark>(ManualConfig.Create(DefaultConfig.Instance.WithArtifactsPath($"{projectDirectory}/{assemblyName}/Reports/{arg}")).AddJob(Job.ShortRun));
                    break;
                case "All":
                    // BenchmarkRunner.Run<HttpClientBenchmark>(ManualConfig.Create(DefaultConfig.Instance.WithArtifactsPath($"{projectDirectory}/{assemblyName}/Reports")).AddJob(Job.ShortRun));
                    // BenchmarkRunner.Run<RefitBenchmark>(ManualConfig.Create(DefaultConfig.Instance.WithArtifactsPath($"{projectDirectory}/{assemblyName}/Reports")).AddJob(Job.ShortRun));
                    // BenchmarkRunner.Run<RestSharpBenchmark>(ManualConfig.Create(DefaultConfig.Instance.WithArtifactsPath($"{projectDirectory}/{assemblyName}/Reports")).AddJob(Job.ShortRun));
                    break;
        
            }
        }
    }

    private static void RuntimesHandler(ManualConfig? config, string[] args)
    {
        foreach (var arg in args)
        {
            if (arg == "--runtimes")
            {
                var next = Array.IndexOf(args, arg) + 1;
                while (args[next] != "--filter")
                {
                    if (config != null) SetRuntimes(args[next], config);
                    next++;
                }
                break;
            }
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


