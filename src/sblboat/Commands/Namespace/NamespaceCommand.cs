using System.CommandLine;
using sblboat.Commands.Namespace.SubCommands;

namespace sblboat.Commands.Namespace;

public class NamespaceCommand : Command
{
    private const string CommandName = "namespace";
    private const string CommandDescription = "Connect, Add or Remove an Namespace";

    public NamespaceCommand() : base(CommandName, CommandDescription)
    {
        AddCommand(new ConnectCommand());
    }
}