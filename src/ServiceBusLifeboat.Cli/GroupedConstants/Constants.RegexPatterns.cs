namespace ServiceBusLifeboat.Cli.GroupedConstants;

public static partial class Constants
{
    public static class RegexPatterns
    {
        public const string ServiceBuConnectionStringPattern = @"^Endpoint=(?<Endpoint>[^;]+);SharedAccessKeyName=(?<KeyName>[^;]+);SharedAccessKey=(?<Key>[^;]+)$";
        public const string ConfigurationFileBodyPattern = @"^\{""FileName"":""[^""]+"",""Path"":""[^""]+"",""Content"":""[^""]+""\}$";
    }
}