using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using ServiceBusLifeboat.Cli.Enums;
using static ServiceBusLifeboat.Cli.Enums.CustomLogLevel;
using ServiceBusLifeboat.Cli.Themes;
using ILogger = Serilog.ILogger;

namespace ServiceBusLifeboat.Cli.Extensions;

internal static class SerilogExtensions
{
    public static void LogSuccessInformation(this ILogger logger, string message)
    {
        var customTheme = SystemConsoleThemeTemplates.Success;
        var level = Success;
        var template = "[SUCCESS] {Message}";

        WriteCustomMessage(level, template, message, customTheme);
    }

    public static void LogSavedInformation(this ILogger logger, string message)
    {
        var customTheme = SystemConsoleThemeTemplates.NotSaved;
        var level = Saved;
        var template = "[RESOURCE SAVED] {Message}";

        WriteCustomMessage(level, template, message, customTheme);
    }

    public static void LogNotSavedWarning(this ILogger logger, string message)
    {
        var level = NotSaved;
        var customTheme = SystemConsoleThemeTemplates.NotSaved;
        var template = "[RESOURCE NOT SAVED] {Message}";

        WriteCustomMessage(level, template, message, customTheme);
    }

    private static void WriteCustomMessage(CustomLogLevel level, string template, string message, SystemConsoleTheme theme)
    {
        using (var log = new LoggerConfiguration().WriteTo.Console(theme: theme).CreateLogger())
        {
            switch (level)
            {
                case Success or Saved:
                    log.Information(template, message);
                    break;

                case NotSaved:
                    log.Warning(template, message);
                    break;
            }
        }
    }
}
