namespace ServiceBusLifeboat.Cli.Services.Encryption;

public interface IEncryptionService
{
    string EncryptString(string input, string encryptionKey);
    string DecryptString(string input, string encryptionKey);
}