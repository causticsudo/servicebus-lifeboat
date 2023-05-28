namespace ServiceBusLifeboat.Cli.GroupedConstants;

public static class RegexPattern
{
    public const string ServiceBuConnectionStringPattern = @"^Endpoint=(?<Endpoint>[^;]+);SharedAccessKeyName=(?<KeyName>[^;]+);SharedAccessKey=(?<Key>[^;]+)$";
}