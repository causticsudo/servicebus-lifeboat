using ServiceBusLifeboat.Cli.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ServiceBusLifeboat.Cli;

internal class Program
{
    internal static async Task Main(String[] args)
    {
        IHost host = CreateHostBuilder(args).Build();

        await host.Services
             .BuildRootCommand()
             .InvokeAsync(args);
    }

    internal static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                services.AddCommands();
                services.AddCommandHandlers();

                var logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console()
                    .CreateLogger();

                services.AddSingleton<Serilog.ILogger>(logger);
                services.AddLogging(builder => builder.SetMinimumLevel(LogLevel.Debug).AddSerilog());
            })
            .UseConsoleLifetime();
}