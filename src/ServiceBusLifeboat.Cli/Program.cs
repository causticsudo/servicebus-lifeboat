using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.CommandLine;
using ServiceBusLifeboat.Cli.Actions.Namespace.Commands;
using ServiceBusLifeboat.Cli.Actions.Namespace.SubCommands;
using ServiceBusLifeboat.Cli.Extensions;

namespace ServiceBusLifeboat.Cli;

internal class Program
{
    private const string DefaultDescription = "A simple AzureServiceBus command line interface.";

    internal static async Task Main(String[] args)
    {
         var rootCommand = new ServiceCollection()
             .ConfigureServices()
             .GetRootCommand();

         await rootCommand.InvokeAsync(args);
    }
}