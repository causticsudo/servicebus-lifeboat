using static ServiceBusLifeboat.Cli.GroupedConstants.Constants.ApplicationInformations;

namespace ServiceBusLifeboat.Cli.Services.Configuration.Namespace;

public class ConnectionState : IAppConfiguration
{
    public string FileName { get; set; } = DefaultConnectionStateFile;
    public string Path { get; set; } = string.Empty;
    public string Content { get; set; }  = string.Empty;
}