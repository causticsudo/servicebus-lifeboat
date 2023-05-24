using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using ServiceBusLifeboat.Console.Extensions;
using static System.Console;

namespace ServiceBusLifeboat.Console;

internal class Program
{
    private static readonly RootCommand _rootCommand = new();
    private static readonly ServiceCollection _services = new();

    internal async static void Main(String[] args)
    {
        ConfigureServices();

        using (var serviceProvider = _services.BuildServiceProvider())
        {

            _rootCommand.RegisterCommands(serviceProvider);

            bool result;
            do
            {
                await _rootCommand.InvokeAsync(args);

                var response = ReadLine();

                result = (response == "1");

            } while (result);
        }

        System.Console.WriteLine("out of service");
        ReadLine();
    }

    private static void ConfigureServices()
    {
        _services.AddCommands();
        _services.AddCommandHandlers();
    }
}