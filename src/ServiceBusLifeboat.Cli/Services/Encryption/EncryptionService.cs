using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace ServiceBusLifeboat.Cli.Services.Encryption;

public class EncryptionService : IEncryptionService
{
    public string EncryptString(string input, string encryptionKey)
    {
        ValidateInput(input, encryptionKey);

        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        byte[] encryptionKeyBytes = Encoding.UTF8.GetBytes(encryptionKey);

        IBufferedCipher cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine()));
        cipher.Init(true, new KeyParameter(encryptionKeyBytes));

        byte[] encryptedBytes = new byte[cipher.GetOutputSize(inputBytes.Length)];
        int processedBytes = cipher.ProcessBytes(inputBytes, 0, inputBytes.Length, encryptedBytes, 0);
        cipher.DoFinal(encryptedBytes, processedBytes);

        return Convert.ToBase64String(encryptedBytes);
    }

    public string DecryptString(string input, string encryptionKey)
    {
        ValidateInput(input, encryptionKey);

        byte[] inputBytes = Convert.FromBase64String(input);
        byte[] encryptionKeyBytes = Encoding.UTF8.GetBytes(encryptionKey);

        IBufferedCipher cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine()));
        cipher.Init(false, new KeyParameter(encryptionKeyBytes));

        byte[] decryptedBytes = new byte[cipher.GetOutputSize(inputBytes.Length)];
        int processedBytes = cipher.ProcessBytes(inputBytes, 0, inputBytes.Length, decryptedBytes, 0);
        cipher.DoFinal(decryptedBytes, processedBytes);

        return Encoding.UTF8.GetString(decryptedBytes);
    }

    private void ValidateInput(string input, string encryptionKey)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentNullException(nameof(input));

        if (string.IsNullOrEmpty(encryptionKey))
            throw new ArgumentNullException(nameof(encryptionKey));
    }
}