using Serilog.Sinks.SystemConsole.Themes;
using static System.ConsoleColor;

namespace ServiceBusLifeboat.Cli.Themes;

public static class SystemConsoleThemeTemplates
{
    public static SystemConsoleTheme Success { get; } = new(new Dictionary<ConsoleThemeStyle, SystemConsoleThemeStyle>
    {
        {
            ConsoleThemeStyle.LevelInformation,
            new SystemConsoleThemeStyle { Foreground = White, Background = Green }
        },
        {
            ConsoleThemeStyle.Text,
            new SystemConsoleThemeStyle { Foreground = Green }
        },
        {
            ConsoleThemeStyle.String,
            new SystemConsoleThemeStyle { Foreground = White }
        }
    });

    public static SystemConsoleTheme Saved { get; } = new(new Dictionary<ConsoleThemeStyle, SystemConsoleThemeStyle>
    {
        {
            ConsoleThemeStyle.LevelInformation,
            new SystemConsoleThemeStyle { Foreground = White, Background = Blue }
        },
        {
            ConsoleThemeStyle.Text,
            new SystemConsoleThemeStyle { Foreground = Blue, }
        },
        {
            ConsoleThemeStyle.String,
            new SystemConsoleThemeStyle { Foreground = White }
        }
    });

    public static SystemConsoleTheme NotSaved { get; } = new(new Dictionary<ConsoleThemeStyle, SystemConsoleThemeStyle>
    {
        {
            ConsoleThemeStyle.LevelWarning,
            new SystemConsoleThemeStyle { Foreground = White, Background = Yellow}
        },
        {
            ConsoleThemeStyle.Text,
            new SystemConsoleThemeStyle { Foreground = Yellow, }
        },
        {
            ConsoleThemeStyle.String,
            new SystemConsoleThemeStyle { Foreground = White }
        },
    });
}