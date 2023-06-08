using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Serilog;
using ServiceBusLifeboat.Cli.Extensions;
using static System.AppDomain;
using static ServiceBusLifeboat.Cli.GroupedConstants.Constants;

namespace ServiceBusLifeboat.Cli.Services.File;

public class FileService : IFileService
{
    private readonly ILogger _logger;

    public FileService(ILogger logger)
    {
        _logger = logger;
    }

    public T? GetJsonFile<T>(string filePath) where T : IFile, new()
    {
        T? file = default(T);

        if (FileAlreadyExists(filePath))
        {
            string configContent = System.IO.File.ReadAllText(filePath);
            file = JsonConvert.DeserializeObject<T>(configContent);

            return file;
        }

        return file;
    }

    public bool IsMatchFileFound(string regexPattern, string filePath)
    {
        if (!FileAlreadyExists(filePath))
        {
            return false;
        }

        string fileContent;
        using (var reader = new StreamReader(filePath))
        {
            fileContent = reader.ReadToEnd();
        }

        return Regex.IsMatch(fileContent, regexPattern);
    }

    public string GetFilePath(string folderName, string fileName)
    {
        string filePath = (EnvironmentExtensions.IsUnix())
            ? $"{folderName}/{fileName}"
            : $"{folderName}\\{fileName}";

        return Path.Combine(CurrentDomain.BaseDirectory, filePath);
    }

    public void CreateJsonFile(IFile? file, string filePath)
    {
        try
        {
            var directory = Path.GetDirectoryName(filePath);
            var serializedFile = JsonConvert.SerializeObject(file);

            Directory.CreateDirectory(directory ??
                                      throw new InvalidOperationException(FileServiceMessages.InvalidFilePath));

            using (var writer = new StreamWriter(filePath))
            {
                writer.Write(serializedFile);
            }
        }
        catch (InvalidOperationException ex)
        {
            _logger.Error(ex.Message);
        }
        catch (Exception)
        {
            _logger.Error(FileServiceMessages.FileUpdateFailed);
        }
    }

    public void UpdateJsonFile<T>(IFile? file, string filePath) where T : IFile, new()
    {
        try
        {
            var newSerializedFile = JsonConvert.SerializeObject(file);
            var directory = Path.GetDirectoryName(filePath);
            Directory.CreateDirectory(directory ??
                                      throw new InvalidOperationException(FileServiceMessages.InvalidFilePath));

            using (var writer = new StreamWriter(filePath))
            {
                writer.Write(newSerializedFile);
            }
        }
        catch (InvalidOperationException ex)
        {
            _logger.Error(ex.Message);
        }
        catch (Exception)
        {
            _logger.Error(FileServiceMessages.FileUpdateFailed);
        }
    }

    private bool FileAlreadyExists(string filePath) =>
        System.IO.File.Exists(filePath);
}