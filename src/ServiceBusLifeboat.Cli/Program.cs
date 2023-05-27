using System.CommandLine;
using ServiceBusLifeboat.Cli.Commands.Namespace;

namespace ServiceBusLifeboat.Cli;

internal class Program
{
    private const string DefaultDescription = "A simple AzureServiceBus command line interface.";

    internal static async Task Main(String[] args)
    {
        var rootCommand = new RootCommand(DefaultDescription);

        rootCommand.AddCommand(new NamespaceCommand());

        await rootCommand.InvokeAsync(args);
    }
}