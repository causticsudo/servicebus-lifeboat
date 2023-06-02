using ServiceBusLifeboat.Cli.Services.File;

namespace ServiceBusLifeboat.Cli.Services.Configuration;

public interface IConfigurationService
{
    IFile ResolveConfigurationFile<T>() where T : IFile, new();
    void UpdateConfigurationContent<T>(IFile configurationFile, string content) where T : IFile, new();
}