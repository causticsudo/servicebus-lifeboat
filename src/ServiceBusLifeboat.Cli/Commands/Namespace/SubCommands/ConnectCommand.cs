using System.CommandLine;
using System.Text.Json;
using System.Text.RegularExpressions;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Serilog;
using ServiceBusLifeboat.Cli.Binders;
using ServiceBusLifeboat.Cli.DependencyInjection;
using ServiceBusLifeboat.Cli.Extensions;
using ServiceBusLifeboat.Cli.Model;

namespace ServiceBusLifeboat.Cli.Commands.Namespace.SubCommands;

public class ConnectCommand : Command
{
    private const string CommandName = "conn";
    private const string CommandDescription = "Enter a new namespace connection and set it to the current namespace";

    private static ServiceBusAdministrationClient _adminClient;
    private static ServiceBusClient _client;

    public ConnectCommand() :base(CommandName, CommandDescription)
    {
        var saveOption = new Option<bool>(
            aliases: new []{ "--save", "-s"},
            description: "Set current namespace and save to namespace store");

        var nameOption =  new Option<string>(
            aliases: new []{ "--name", "-n"},
            description: "Namespace custom name. If false, the endpoint name will be set as the default value");

        var connectionStringOption =  new Option<string?>(
            aliases: new []{ "--connection-string", "-cs"},
            description: "Namespace connection string",
            isDefault: true,
            parseArgument: _ =>
            {
                //Todo: we need a complex validator
                if (_.Tokens.Count == 0)
                {
                    _.ErrorMessage = $"[--connection-string] option is required to [{CommandName}] command";
                    return null!;
                }

                var connectionString = _.Tokens.Single().Value;
                if (connectionString.IsNullOrWhiteSpace())
                {
                    _.ErrorMessage = $"[--connection-string] value should not be null or empty";
                    return null!;
                }

                if (ValidateConnectionString(connectionString))
                {
                    return connectionString;
                }
                else
                {
                    _.ErrorMessage = $"[--connection-string] is in the wrong format";
                    return null!;
                }
            }){ IsRequired = true };

        AddOption(saveOption);
        AddOption(nameOption);
        AddOption(connectionStringOption);

        this.SetHandler(async (saveOptionValue, serviceBusNamespaceValue, logger) =>
        {
            await Handle(saveOptionValue, serviceBusNamespaceValue, logger);
        },
            saveOption,
            new ServiceBusNamespaceBinder(nameOption, connectionStringOption!),
            new SerilogBinder());
    }


    private Task Handle(bool mustSave, ServiceBusNamespace serviceBusNamespace, ILogger logger)
    {
        var namespaceConnectionString = serviceBusNamespace.ConnectionString;

        try
        {
            logger.Information("Validating connect-string on AzureServiceBus...");

            _adminClient = new(namespaceConnectionString);
            _client = new(namespaceConnectionString);

            logger.Success("Successful connection validation");
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
        }

        var name = serviceBusNamespace.Name;
        var id = serviceBusNamespace.Id;
        var connectionString = serviceBusNamespace.ConnectionString.MaskAfterCharCount('/', 3, 64);

        var serviceBusNamespaceJson = JsonSerializer.Serialize(new
        {
            name,
            id,
            connectionString
        });

        switch (mustSave)
        {
            case true:
                SaveEntity(serviceBusNamespace);
                logger.Stored(serviceBusNamespaceJson);
                return Task.CompletedTask;

            case false:
                logger.NotStored(serviceBusNamespaceJson);
                break;
        }

        return Task.CompletedTask;
    }

    private void SaveEntity(ServiceBusNamespace serviceBusNamespace)
    {
        //Ver se já existe o arquivo, se não cria um
        
        //Serializar o objeto de configuração
        //public
    }

    private static bool ValidateConnectionString(string connectionString)
    {
        var pattern = @"^Endpoint=(?<Endpoint>[^;]+);SharedAccessKeyName=(?<KeyName>[^;]+);SharedAccessKey=(?<Key>[^;]+)$";

        var regex = new Regex(pattern);

        var match = regex.Match(connectionString);

        return match.Success;
    }
}