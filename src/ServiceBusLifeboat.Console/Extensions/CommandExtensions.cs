using System.CommandLine;

namespace ServiceBusLifeboat.Console.Extensions;

/// <summary>
/// Extensions method of <see cref="Command"/> class
/// </summary>
public static class CommandExtensions
{
    /// <summary>
    ///  Adds an array of <see cref="Option"/> to the command.
    /// </summary>
    /// <param name="command">The command instance.</param>
    /// <param name="options">The option to add to the command.</param>
    public static void AddOptions(this Command command, Option[] options)
    {
        foreach (var opt in options)
        {
            command.AddOption(opt);
        }
    }
}