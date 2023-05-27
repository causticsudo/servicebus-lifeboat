using System.CommandLine;

namespace ServiceBusLifeboat.Cli;

internal class Program
{
    private const string DefaultDescription = "A simple AzureServiceBus command line interface.";

    internal static async Task Main(String[] args)
    {
        var rootCommand = new RootCommand(DefaultDescription);

        await rootCommand.InvokeAsync(args);
    }
}