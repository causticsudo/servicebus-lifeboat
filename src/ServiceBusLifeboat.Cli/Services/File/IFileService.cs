namespace ServiceBusLifeboat.Cli.Services.File;

public interface IFileService
{
    void CreateJsonFile(IFile? file, string filePath);
    void UpdateJsonFile<T>(IFile? file, string filePath) where T : IFile, new();
    T? GetJsonFile<T>(string filePath) where T : IFile, new();
    bool IsMatchFileFound(string regexPattern, string filePath);
    string GetFilePath(string folderName, string fileName);
}