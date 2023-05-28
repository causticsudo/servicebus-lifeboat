using System.Text.Json;
using System.Text.Json.Nodes;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Serilog;
using ServiceBusLifeboat.Cli.Extensions;
using ServiceBusLifeboat.Cli.Model;

namespace ServiceBusLifeboat.Cli.Actions.Namespace.SubCommands;

public class ConnectCommandHandler
{
    private ServiceBusAdministrationClient _adminClient;
    private ServiceBusClient _client;

    private readonly ILogger _logger;

    public ConnectCommandHandler(ILogger logger)
    {
        _logger = logger;
    }

    public async Task Handle(bool mustSave, ServiceBusNamespace serviceBusNamespace)
    {
        var namespaceConnectionString = serviceBusNamespace.ConnectionString;

        TryBuildServiceBusClient(namespaceConnectionString);

        var serviceBusNamespaceJson = GetJsonObjectString(serviceBusNamespace);

        switch (mustSave)
        {
            case true:
                SaveEntity(serviceBusNamespace);
                _logger.Stored(serviceBusNamespaceJson);
                break;

            case false:
                _logger.NotStored(serviceBusNamespaceJson);
                break;
        }
    }

    private void TryBuildServiceBusClient(string connectionString)
    {
        try
        {
            _logger.Information("Validating connect-string on AzureServiceBus...");

            _adminClient = new ServiceBusAdministrationClient(connectionString);
            _client = new ServiceBusClient(connectionString);

            _logger.Success("Successful connection validation");
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
        }
    }

    private string GetJsonObjectString(ServiceBusNamespace serviceBusNamespace)
    {
        var name = serviceBusNamespace.Name;
        var id = serviceBusNamespace.Id;
        var secretString = serviceBusNamespace.ConnectionString.MaskAfterCharCount('/', 3, 64);

        return JsonSerializer.Serialize(new
        {
            name,
            id,
            connectionString = secretString
        });
    }

    private void SaveEntity(ServiceBusNamespace serviceBusNamespace)
    {
        // Implemente a lógica para salvar a entidade
    }
}
