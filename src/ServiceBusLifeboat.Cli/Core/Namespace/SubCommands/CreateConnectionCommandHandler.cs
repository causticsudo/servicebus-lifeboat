using System.Security;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Serilog;
using ServiceBusLifeboat.Cli.Extensions;
using ServiceBusLifeboat.Cli.Extensions.Serilog;
using ServiceBusLifeboat.Cli.PromptModels;
using ServiceBusLifeboat.Cli.Services.Configuration;
using ServiceBusLifeboat.Cli.Services.Configuration.Namespace;
using Sharprompt;

namespace ServiceBusLifeboat.Cli.Core.Namespace.SubCommands;

public class CreateConnectionCommandHandler
{
    private ServiceBusAdministrationClient _adminClient;
    private ServiceBusClient _client;

    private readonly ILogger _logger;
    private readonly IConfigurationService _configurationService;

    public CreateConnectionCommandHandler(ILogger logger, IConfigurationService configurationService)
    {
        _logger = logger;
        _configurationService = configurationService;
    }

    public async Task Handle(bool mustSave)
    {
        var config = _configurationService.ResolveConfigurationFile<ConnectionState>();
        var connectionString = Prompt.Bind<CreateConnectionPromptModel>().GetSecureString();

        using (var insecureString = connectionString)
        {
            _configurationService.UpdateConfigurationContent<ConnectionState>(config, insecureString.ToStringSafely());
        }

        // TryBuildServiceBusClient(connectionString);
        // config.Content = connectionString.ToStringSafely();
    }

    private void TryBuildServiceBusClient(SecureString connectionString)
    {
        try
        {
            _logger.Information("Validating connect-string on AzureServiceBus...");

            using (var secureString = connectionString)
            {
                _adminClient = new ServiceBusAdministrationClient(secureString.ToStringSafely());
                _client = new ServiceBusClient(secureString.ToStringSafely());
            }

            _logger.SuccessInformation($"Successful connection validation");
        }
        catch(FormatException ex)
        {
            _logger.Error(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
        }
    }
}
