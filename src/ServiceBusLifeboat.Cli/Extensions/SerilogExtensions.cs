using Serilog;
using ServiceBusLifeboat.Cli.Themes;
using ILogger = Serilog.ILogger;

namespace ServiceBusLifeboat.Cli.Extensions;

public static class SerilogExtensions
{
    public static void Success(this ILogger logger, string message)
    {
        var customTheme = SystemConsoleThemeTemplates.Success;

        new LoggerConfiguration()
            .WriteTo.Console(theme: customTheme)
            .CreateLogger()
            .Information("[SUCCESS] {Message}", message);

        Log.CloseAndFlush();
    }

    public static void Stored(this ILogger logger, string message)
    {
        var customTheme = SystemConsoleThemeTemplates.Stored;

        new LoggerConfiguration()
            .WriteTo.Console(theme: customTheme)
            .CreateLogger()
            .Information("[RESOURCE STORED] {Message}", message);

        Log.CloseAndFlush();
    }

    public static void NotStored(this ILogger logger, string message)
    {
        var customTheme = SystemConsoleThemeTemplates.NotStored;

        new LoggerConfiguration()
            .WriteTo.Console(theme: customTheme)
            .CreateLogger()
            .Warning("[RESOURCE NOT STORED] {Message}", message);

        Log.CloseAndFlush();
    }
}