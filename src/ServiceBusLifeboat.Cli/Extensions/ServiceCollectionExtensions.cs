using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using ServiceBusLifeboat.Cli.Actions.Namespace.Commands;
using ServiceBusLifeboat.Cli.Actions.Namespace.SubCommands;

namespace ServiceBusLifeboat.Cli.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCommands(this IServiceCollection services)
    {
        services.AddSingleton<RootCommand>();
        services.AddSingleton<NamespaceCommand>();
        services.AddSingleton<CreateConnectionCommand>();
    }

    public static void AddCommandHandlers(this IServiceCollection services)
    {
        services.AddTransient<CreateConnectionCommandHandler>();
    }
}