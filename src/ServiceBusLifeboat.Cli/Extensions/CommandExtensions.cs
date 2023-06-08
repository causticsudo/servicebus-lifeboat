using System.CommandLine;

namespace ServiceBusLifeboat.Cli.Extensions;

public static class CommandExtensions
{
    public static void AddOptions(this Command command, params Symbol[] options)
    {
        foreach (var item in options)
        {
            Option? option = item as Option;

            if (option is not null)
                command.AddOption(option);
        }
    }
}