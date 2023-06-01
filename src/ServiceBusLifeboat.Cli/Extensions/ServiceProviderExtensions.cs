using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using ServiceBusLifeboat.Cli.Actions.Namespace.Commands;
using static ServiceBusLifeboat.Cli.GroupedConstants.ApplicatioInformation;

namespace ServiceBusLifeboat.Cli.Extensions;

public static class ServiceProviderExtensions
{
    public static RootCommand BuildRootCommand(this IServiceProvider? serviceProvider)
    {
        var rootCommand = new RootCommand(DefaultApplicationDescription);

        var namespaceCommand = serviceProvider?.GetRequiredService<NamespaceCommand>();
        rootCommand.AddCommand(namespaceCommand!);

        return rootCommand;
    }
}