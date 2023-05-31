using ServiceBusLifeboat.Cli.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace ServiceBusLifeboat.Cli;

internal class Program
{
    internal static async Task Main(String[] args)
    {
         var rootCommand = new ServiceCollection()
             .ConfigureServices()
             .GetRootCommand();

         await rootCommand.InvokeAsync(args);
    }
}