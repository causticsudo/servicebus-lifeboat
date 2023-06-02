namespace ServiceBusLifeboat.Cli.Services.File;

public interface IFileService
{
    void CreateJsonFile<T>(string filePath) where T : IFile, new();
    void UpdateJsonFile<T>(IFile file, string filePath) where T : IFile, new();
    IFile TryGetJsonFile<T>(string filePath) where T : IFile, new();
    bool IsMatchFileFound(string regexPattern, string filePath);
    string GetFilePath(string folderName, string fileName);
}