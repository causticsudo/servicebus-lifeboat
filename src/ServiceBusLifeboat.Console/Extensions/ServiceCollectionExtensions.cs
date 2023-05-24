using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using ServiceBusLifeboat.Console.CommandHandlers;
using ServiceBusLifeboat.Console.Commands;

namespace ServiceBusLifeboat.Console.Extensions;

/// <summary>
/// Extension class from ServiceCollection methods
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Insert application commands on dependency injection container
    /// </summary>
    /// <param name="services" cref="IServiceCollection"></param>
    public static void AddCommands(this IServiceCollection services)
    {
        services.AddScoped<Command, ConnectOnNamespace>();
    }

    /// <summary>
    /// Insert application command handlers on dependency injection container
    /// </summary>
    /// <param name="services"></param>
    public static void AddCommandHandlers(this IServiceCollection services)
    {
        services.AddSingleton<ConnectOnNamespaceHandler>();
    }
}