using System.CommandLine;
using ServiceBusLifeboat.Cli.Actions.Namespace.SubCommands;

namespace ServiceBusLifeboat.Cli.Actions.Namespace.Commands;

public class NamespaceCommand : Command
{
    private const string CommandName = "namespace";
    private const string CommandDescription = "Connect, Add or Remove an Namespace";

    public NamespaceCommand(CreateConnectionCommand createConnectionCommand) : base(CommandName, CommandDescription)
    {
        AddCommand(createConnectionCommand);
    }
}