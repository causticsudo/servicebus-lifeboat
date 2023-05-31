using System.CommandLine;
using ServiceBusLifeboat.Cli.Extensions;

namespace ServiceBusLifeboat.Cli.Actions.Namespace.SubCommands;

public class CreateConnectionCommand : Command
{
    private const string CommandName = "create-connection";
    private const string CommandDescription = "Set a new namespace connection";

    private readonly CreateConnectionCommandHandler _handler;

    public CreateConnectionCommand(CreateConnectionCommandHandler handler) :base(CommandName, CommandDescription)
    {
        _handler = handler;

        var saveOption = CreateSaveOption();

        this.AddOptions(saveOption);

        this.SetHandler(
            async (saveOptionValue) =>
            {
                await _handler.Handle(saveOptionValue);
            },
            saveOption
        );

    }

    private static Option<bool> CreateSaveOption()
    {
        return new Option<bool>(
            aliases: new []{ "--save", "-s"},
            description: "Set current namespace and save to namespace store");
    }
}