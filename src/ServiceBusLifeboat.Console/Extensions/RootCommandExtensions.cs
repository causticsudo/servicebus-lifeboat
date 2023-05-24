using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBusLifeboat.Console.Extensions;

/// <summary>
/// 
/// </summary>
public static class RootCommandExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="rootCommand"></param>
    /// <param name="serviceProvider"></param>
    public static void RegisterCommands(this RootCommand rootCommand, ServiceProvider serviceProvider)
    {
        var commands = serviceProvider.GetServices<Command>();

        foreach (var command in commands)
        {
            rootCommand.AddCommand(command);
        }
    }
}