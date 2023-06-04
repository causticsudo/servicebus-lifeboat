using Serilog;
using ServiceBusLifeboat.Cli.Extensions;
using ServiceBusLifeboat.Cli.Services.File;
using static ServiceBusLifeboat.Cli.GroupedConstants.Constants;
using static ServiceBusLifeboat.Cli.GroupedConstants.Constants.RegexPatterns;

namespace ServiceBusLifeboat.Cli.Services.Configuration;

public class ConfigurationService : IConfigurationService
{
    private readonly IFileService _fileService;
    private readonly ILogger _logger;

    public ConfigurationService(IFileService fileService, ILogger logger)
    {
        _fileService = fileService;
        _logger = logger;
    }

    public IAppConfiguration? ResolveConfigurationFile<T>() where T : IAppConfiguration, new()
    {
        var fileName = new T().FileName;
        var folderName = ApplicationInformations.DefaultConfigurationFolder;
        var filePath = _fileService.GetFilePath(folderName, fileName);

        if (_fileService.IsMatchFileFound(ConfigurationFileBodyPattern, filePath))
        {
            _logger.Information(ConfigurationServiceMessages.ConfigurationFileFound);

            return _fileService.GetJsonFile<T>(filePath);
        }
        else
        {
            var configFile = new T();
            configFile.Path = filePath;

            _fileService.CreateJsonFile(configFile, filePath);

            _logger.Information(ConfigurationServiceMessages.ConfigurationFileCreated);

            return configFile;
        }
    }

    public void UpdateConfigurationContent<T>(IAppConfiguration? configurationFile, string content) where T : IAppConfiguration, new()
    {
        var fileName = configurationFile.FileName;
        var defaultConfigurationFolderName = ApplicationInformations.DefaultConfigurationFolder;

        var filePath = configurationFile.Path.IsNullOrWhiteSpace()
            ?_fileService.GetFilePath(defaultConfigurationFolderName, fileName)
            : configurationFile.Path;

        try
        {
            configurationFile.Content = content;

            _fileService.UpdateJsonFile<T>(configurationFile, filePath);

            _logger.Information(ConfigurationServiceMessages.SuccesConfigurationFileUpdated);
        }
        catch (Exception e)
        {
            _logger.Error(ConfigurationServiceMessages.SuccesConfigurationFileUpdated, e);
        }
    }
}