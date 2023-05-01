using System;
using Colorify;
using static Colorify.Colors;
using Colorify.UI;
using ToolBox.Bridge;
using ToolBox.Files;
using ToolBox.Notification;
using ToolBox.Platform;
using System.Collections.Generic;

namespace ApiBenchmark.MVC;

public static class ScriptRunner
{
    private static Format _colorify { get; set; }
    public static INotificationSystem _notificationSystem { get; set; }
    public static IBridgeSystem _bridgeSystem { get; set; }
    public static ShellConfigurator _shell { get; set; }

    public static DiskConfigurator _disk { get; set; }
    public static PathsConfigurator _path { get; set; }
    
    public static async Task Run(string host, IEnumerable<string> frameworks, string client)
    {
        try
        {
            _disk = new DiskConfigurator(FileSystem.Default);
            switch (OS.GetCurrent())
            {
                case "win":
                    _path = new PathsConfigurator(CommandSystem.Win, FileSystem.Default);
                    break;
                case "mac":
                    _path = new PathsConfigurator(CommandSystem.Mac, FileSystem.Default);
                    break;
            }

            _notificationSystem = NotificationSystem.Default;
            switch (OS.GetCurrent())
            {
                case "win":
                    _bridgeSystem = BridgeSystem.Bat;
                    _colorify = new Format(Theme.Dark);
                    break;
                case "gnu":
                    _bridgeSystem = BridgeSystem.Bash;
                    _colorify = new Format(Theme.Dark);
                    break;
                case "mac":
                    _bridgeSystem = BridgeSystem.Bash;
                    _colorify = new Format(Theme.Light);
                    break;
            }
            _shell = new ShellConfigurator(_bridgeSystem, _notificationSystem);
            Console.WriteLine("I think it's working however I'm not sure");
            // Menu();
            await RunBenchmarkTest(host, frameworks, client);
            _colorify.ResetColor();
            _colorify.Clear();
        }
        catch (ArgumentOutOfRangeException)
        {
            MessageException("Ahh my eyes! Why this console is too small?");
        }
        catch (Exception ex)
        {
            MessageException(ex.ToString());
        }

        static Task RunBenchmarkTest(string host, IEnumerable<string> runtimes, string client)
        {
            if (runtimes == null || host == null)
            {
                throw new ArgumentNullException("Runtimes or host cannot be null");
            }

            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            string frameworkStringParam = string.Empty;
            if (runtimes.Count() > 1)
            {
                frameworkStringParam = string.Join(" ", runtimes);
            }
            

            if (runtimes.Count() == 1)
            {
                frameworkStringParam = runtimes.First();
            }

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
            
            Console.WriteLine(path);
            Response result = _shell.Term($"sh benchmark.sh {host} \"{frameworkStringParam}\" {client}", Output.Internal, path);
            return Task.CompletedTask;
        }

        static void MessageException(string message)
        {
            _colorify.ResetColor();
            _colorify.Clear();
            _colorify.WriteLine(message, bgDanger);
        }
    }
}