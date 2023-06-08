using System.CommandLine;
using ServiceBusLifeboat.Cli.Core.Namespace.SubCommands;

namespace ServiceBusLifeboat.Cli.Core.Namespace.Commands;

public class NamespaceCommand : Command
{
    private const string CommandName = "namespace";
    private const string CommandDescription = "Connect, Add or Remove an Namespace";

    public NamespaceCommand(CreateConnectionCommand createConnectionCommand) : base(CommandName, CommandDescription)
    {
        AddCommand(createConnectionCommand);
    }
}