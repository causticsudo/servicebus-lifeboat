using ServiceBusLifeboat.Cli.Services.File;
using static ServiceBusLifeboat.Cli.GroupedConstants.Constants.ApplicationInformations;

namespace ServiceBusLifeboat.Cli.Services.Token;

public class Token : IFile
{
    public string Format { get; set; }
    public DateTime ExpirationDate { get; set; }
    public uint TokenDurationInMinutes { get; set; }
    public string GeneratedBy { get; set; }
    public string FileName { get; set; } = DefaultTokenFile;
    public string Path { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}