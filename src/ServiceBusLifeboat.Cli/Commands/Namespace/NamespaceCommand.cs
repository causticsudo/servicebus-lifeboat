using System.CommandLine;
using ServiceBusLifeboat.Cli.Commands.Namespace.SubCommands;

namespace ServiceBusLifeboat.Cli.Commands.Namespace;

public class NamespaceCommand : Command
{
    private const string CommandName = "namespace";
    private const string CommandDescription = "Connect, Add or Remove an Namespace";

    public NamespaceCommand() : base(CommandName, CommandDescription)
    {
        AddCommand(new ConnectCommand());
    }
}