using System.CommandLine;
using ServiceBusLifeboat.Cli.Actions.Namespace.Validators;
using ServiceBusLifeboat.Cli.Binders;
using ServiceBusLifeboat.Cli.Extensions;

namespace ServiceBusLifeboat.Cli.Actions.Namespace.SubCommands;

public class ConnectCommand : Command
{
    private const string CommandName = "conn";
    private const string CommandDescription = "Enter a new namespace connection and set it to the current namespace";

    private readonly ConnectCommandHandler _handler;

    public ConnectCommand(ConnectCommandHandler handler) :base(CommandName, CommandDescription)
    {
        _handler = handler;

        var (saveOption, nameOption, connectionStringOption) =
            (CreateSaveOption(), CreateNameOption(), CreateConnectionStringOption());

        this.AddOptions(saveOption, nameOption, connectionStringOption);

        this.SetHandler(
            async (saveOptionValue, serviceBusNamespaceValue) =>
            {
                await _handler.Handle(saveOptionValue, serviceBusNamespaceValue);
            },
            saveOption,
            new ServiceBusNamespaceBinder(nameOption, connectionStringOption!)
        );

    }

    private static Option<bool> CreateSaveOption()
    {
        return new Option<bool>(
            aliases: new []{ "--save", "-s"},
            description: "Set current namespace and save to namespace store");
    }

    private static Option<string> CreateNameOption()
    {
        return new Option<string>(
            aliases: new []{ "--name", "-n"},
            description: "Namespace custom name. If false, the endpoint name will be set as the default value");
    }

    private static Option<string?> CreateConnectionStringOption()
    {
        return new Option<string?>(
            aliases: new []{ "--connection-string", "-cs"},
            description: "Namespace connection string",
            isDefault: true,
            parseArgument: _ => new ConnectionStringOptionValidator(_).ParseResult()){ IsRequired = true };
    }
}