using System.Security;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Serilog;
using ServiceBusLifeboat.Cli.Application;
using ServiceBusLifeboat.Cli.Extensions;
using ServiceBusLifeboat.Cli.Model;
using ServiceBusLifeboat.Cli.PromptModels;
using ServiceBusLifeboat.Cli.Services;
using Sharprompt;

namespace ServiceBusLifeboat.Cli.Actions.Namespace.SubCommands;

public class CreateConnectionCommandHandler
{
    private ServiceBusAdministrationClient _adminClient;
    private ServiceBusClient _client;

    private readonly ILogger _logger;

    public CreateConnectionCommandHandler(ILogger logger)
    {
        _logger = logger;
    }

    public async Task Handle(bool mustSave)
    {
        var configurationFile = new FileService(_logger).ResolveConfigurationFile<ConfigurationState>();

        var accessToken = TokenService.GenerateToken();

        var connectionString = Prompt.Bind<CreateConnectionPromptModel>().GetSecureString();
        TryBuildServiceBusClient(connectionString);


        _logger.Information(accessToken);

        //criptografar token com MAC ADDRESS
        //criar arquivo de estado (current connection)

        //
    }

    // var namespaceConnectionString = serviceBusNamespace.ConnectionString;
    //
    // TryBuildServiceBusClient(namespaceConnectionString);
    //
    // var serializedServiceBusNamespace = GetJsonObjectString(serviceBusNamespace);
    //
    //     switch (mustSave)
    // {
    //     case true:
    //     SaveEntity(serviceBusNamespace);
    //     _logger.Stored(serializedServiceBusNamespace);
    //     break;
    //
    //     case false:
    //     _logger.NotStored(serializedServiceBusNamespace);
    //     break;
    // }

    private void TryBuildServiceBusClient(SecureString connectionString)
    {
        try
        {
            _logger.Information("Validating connect-string on AzureServiceBus...");

            _adminClient = new ServiceBusAdministrationClient(connectionString.ToSafeString());
            _client = new ServiceBusClient(connectionString.ToSafeString());

            _logger.LogSuccessInformation($"Successful connection validation");
        }
        catch (Exception ex) when(ex is FormatException)
        {
            _logger.Error(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
        }
    }

    // private string GetJsonObjectString(ServiceBusNamespace serviceBusNamespace)
    // {
    //     var name = serviceBusNamespace.Name;
    //     var id = serviceBusNamespace.Id;
    //     var secretString = serviceBusNamespace.ConnectionString.MaskAfterCharCount('/', 3, 64);
    //
    //     return JsonSerializer.Serialize(new
    //     {
    //         name,
    //         id,
    //         connectionString = secretString
    //     });
    // }

    private void SaveEntity(ServiceBusNamespace serviceBusNamespace)
    {
        // Implemente a lógica para salvar a entidade
    }
}
