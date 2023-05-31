using System.Security;
using System.Text;

namespace ServiceBusLifeboat.Cli.Extensions;

public static class StringExtensions
{

    public static bool IsNullOrWhiteSpace(this string input) =>
        String.IsNullOrWhiteSpace(input);

    public static string FormatMacAddress(this string input) =>
        input.Replace(":", "").Replace("-", "");

    public static string ToBase64String(this string input)
    {
        var inputInBytes = Encoding.UTF8.GetBytes(input);

        return Convert.ToBase64String(inputInBytes);
    }

    public static SecureString ConvertToSecureString(this string input)
    {
        var secureString = new SecureString();

        foreach (char c in input)
        {
            secureString.AppendChar(c);
        }

        secureString.MakeReadOnly();

        return secureString;
    }
}