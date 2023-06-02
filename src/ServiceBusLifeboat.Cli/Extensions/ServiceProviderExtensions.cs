using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using ServiceBusLifeboat.Cli.Core.Namespace.Commands;
using static ServiceBusLifeboat.Cli.GroupedConstants.Constants;

namespace ServiceBusLifeboat.Cli.Extensions;

public static class ServiceProviderExtensions
{
    public static RootCommand BuildRootCommand(this IServiceProvider? serviceProvider)
    {
        var rootCommand = new RootCommand(ApplicationInformations.Description);

        var namespaceCommand = serviceProvider?.GetRequiredService<NamespaceCommand>();
        rootCommand.AddCommand(namespaceCommand!);

        return rootCommand;
    }
}