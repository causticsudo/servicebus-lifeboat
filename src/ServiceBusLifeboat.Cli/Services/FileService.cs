using Newtonsoft.Json;
using Serilog;
using ServiceBusLifeboat.Cli.Extensions;

namespace ServiceBusLifeboat.Cli.Services;

public class FileService
{
    private readonly ILogger _logger;

    public FileService(ILogger logger)
    {
        _logger = logger;
    }

    public T ResolveConfigurationFile<T>() where T : new()
    {
        T configFile;

        var rootPath = AppDomain.CurrentDomain.BaseDirectory;
        var configFilePath = Path.Combine(rootPath, "config/.config.json");

        if (File.Exists(configFilePath))
        {
            _logger.Warning("Configuration file has been detected");

            string configContent = File.ReadAllText(configFilePath);
            configFile = JsonConvert.DeserializeObject<T>(configContent);

            return configFile ?? throw new OperationCanceledException();
        }
        else
        {
            try
            {
                configFile = new();
                var configContent = JsonConvert.SerializeObject(configFile);
                var directory = Path.GetDirectoryName(configFilePath);
                Directory.CreateDirectory(directory ?? throw new InvalidOperationException("Invalid directory"));

                using (StreamWriter writer = new StreamWriter(configFilePath))
                {
                    writer.Write(configContent);
                }

                _logger.Success("Configuration file created with success");

                return configFile;
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error(ex.Message);
            }
            catch (Exception)
            {
                _logger.Error("Configuration file cannot be created");
            }
        }

        throw new InvalidOperationException();
    }
}