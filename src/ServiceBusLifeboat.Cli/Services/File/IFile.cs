namespace ServiceBusLifeboat.Cli.Services.File;

public interface IFile
{
    string FileName { get; set; }
    string Path { get; set; }
    string Content { get; set; }
}