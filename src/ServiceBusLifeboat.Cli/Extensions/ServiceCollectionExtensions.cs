using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using ServiceBusLifeboat.Cli.Actions.Namespace.Commands;
using ServiceBusLifeboat.Cli.Actions.Namespace.SubCommands;

namespace ServiceBusLifeboat.Cli.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceProvider ConfigureServices(this IServiceCollection services)
    {
        AddSerilog(services);
        AddCommands(services);
        AddCommandHandlers(services);

        return services.BuildServiceProvider();
    }

    private static void AddSerilog(IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .CreateLogger();

        services.AddSingleton<Serilog.ILogger>(_ => Log.Logger);
        services.AddLogging(builder => builder.SetMinimumLevel(LogLevel.Debug).AddSerilog());
    }

    private static void AddCommands(IServiceCollection services)
    {
        services.AddSingleton<NamespaceCommand>();
        services.AddSingleton<CreateConnectionCommand>();
    }

    private static void AddCommandHandlers(IServiceCollection services)
    {
        services.AddTransient<CreateConnectionCommandHandler>();
    }
}