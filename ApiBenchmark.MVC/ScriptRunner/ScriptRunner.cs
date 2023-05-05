using Colorify;
using Colorify.UI;
using ToolBox.Bridge;
using ToolBox.Files;
using ToolBox.Notification;
using ToolBox.Platform;
using static Colorify.Colors;

namespace ApiBenchmark.MVC.ScriptRunner;

public static class ScriptRunner
{
    private static Format? Colorify { get; set; }
    private static INotificationSystem? NotificationSystem { get; set; }
    private static IBridgeSystem? BridgeSystem { get; set; }
    private static ShellConfigurator? Shell { get; set; }

    public static DiskConfigurator? Disk { get; set; }
    public static PathsConfigurator? Path { get; set; }
    
    public static async Task Run(string host, string[] frameworks, string client)
    {
        try
        {
            Disk = new DiskConfigurator(FileSystem.Default);
            switch (OS.GetCurrent())
            {
                case "win":
                    Path = new PathsConfigurator(CommandSystem.Win, FileSystem.Default);
                    break;
                case "mac":
                    Path = new PathsConfigurator(CommandSystem.Mac, FileSystem.Default);
                    break;
            }

            NotificationSystem = ToolBox.Notification.NotificationSystem.Default;
            switch (OS.GetCurrent())
            {
                case "win":
                    BridgeSystem = ToolBox.Bridge.BridgeSystem.Bat;
                    Colorify = new Format(Theme.Dark);
                    break;
                case "gnu":
                    BridgeSystem = ToolBox.Bridge.BridgeSystem.Bash;
                    Colorify = new Format(Theme.Dark);
                    break;
                case "mac":
                    BridgeSystem = ToolBox.Bridge.BridgeSystem.Bash;
                    Colorify = new Format(Theme.Light);
                    break;
            }
            Shell = new ShellConfigurator(BridgeSystem, NotificationSystem);
            Console.WriteLine("I think it's working however I'm not sure");
            await RunBenchmarkTest(host, frameworks, client);
            Colorify?.ResetColor();
            Colorify?.Clear();
        }
        catch (ArgumentOutOfRangeException)
        {
            MessageException("Ahh my eyes! Why this console is too small?");
        }
        catch (Exception ex)
        {
            MessageException(ex.ToString());
        }

        static Task RunBenchmarkTest(string host, string[] runtimes, string client)
        {
            if (runtimes == null || host == null)
            {
                throw new ArgumentNullException("host");
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
                string? parentDirectory = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(currentDirectory));
                Console.WriteLine($"Parent: {parentDirectory}");
                path = $"{parentDirectory}/ApiBenchmark/ApiBenchmark.BenchmarkTests";
            }
            else
            {
                path = "/app/src/ApiBenchmark/ApiBenchmark.BenchmarkTests";
            }
            
            Console.WriteLine(path);
            Shell?.Term($"sh benchmark.sh {host} \"{frameworkStringParam}\" {client}", Output.Internal, path);
            return Task.CompletedTask;
        }

        static void MessageException(string message)
        {
            Colorify?.ResetColor();
            Colorify?.Clear();
            Colorify?.WriteLine(message, bgDanger);
        }
    }
}