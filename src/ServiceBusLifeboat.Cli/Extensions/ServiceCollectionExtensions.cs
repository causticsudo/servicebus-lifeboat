using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using ServiceBusLifeboat.Cli.Core.Namespace.Commands;
using ServiceBusLifeboat.Cli.Core.Namespace.SubCommands;
using ServiceBusLifeboat.Cli.Services.Configuration;
using ServiceBusLifeboat.Cli.Services.File;
using ServiceBusLifeboat.Cli.Services.NetworkInterface;
using ServiceBusLifeboat.Cli.Services.Token;

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

    public static void AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IFileService, FileService>();
        services.AddSingleton<INetworkInterfaceService, NetworkInterfaceService>();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<IConfigurationService, ConfigurationService>();
    }
}