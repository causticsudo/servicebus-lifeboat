using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using ServiceBusLifeboat.Cli.Extensions.Serilog.Enums;
using ServiceBusLifeboat.Cli.Themes;
using static ServiceBusLifeboat.Cli.Extensions.Serilog.Enums.CustomLogLevel;
using ILogger = Serilog.ILogger;

namespace ServiceBusLifeboat.Cli.Extensions.Serilog;

internal static class SerilogExtensions
{
    private static readonly LoggerConfiguration _loggerConfiguration = new();

    public static void SuccessInformation(this ILogger logger, string message)
    {
        var customTheme = SystemConsoleThemeTemplates.Success;
        var level = Success;
        var template = "[SUCCESS] {Message}";

        WriteCustomMessage(level, template, message, customTheme);
    }

    public static void SavedInformation(this ILogger logger, string message)
    {
        var customTheme = SystemConsoleThemeTemplates.NotSaved;
        var level = Saved;
        var template = "[RESOURCE SAVED] {Message}";

        WriteCustomMessage(level, template, message, customTheme);
    }

    public static void NotSavedWarning(this ILogger logger, string message)
    {
        var level = NotSaved;
        var customTheme = SystemConsoleThemeTemplates.NotSaved;
        var template = "[RESOURCE NOT SAVED] {Message}";

        WriteCustomMessage(level, template, message, customTheme);
    }

    private static void WriteCustomMessage(CustomLogLevel level, string template, string message, SystemConsoleTheme theme)
    {
        var log = _loggerConfiguration.WriteTo.Console(theme: theme).CreateLogger();

        switch (level)
        {
            case Success or Saved:
                log.Information(template, message);
                break;

            case NotSaved:
                log.Warning(template, message);
                break;
        }

        Log.CloseAndFlush();
    }
}