namespace ServiceBusLifeboat.Cli.Services.Configuration;

public interface IConfigurationService
{
    IAppConfiguration? ResolveConfigurationFile<T>() where T : IAppConfiguration, new();
    void UpdateConfigurationContent<T>(IAppConfiguration? configurationFile, string content) where T : IAppConfiguration, new();
}