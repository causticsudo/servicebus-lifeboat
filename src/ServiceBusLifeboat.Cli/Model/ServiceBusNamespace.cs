namespace ServiceBusLifeboat.Cli.Model;

public class ServiceBusNamespace
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string ConnectionString { get; private set; }

    public ServiceBusNamespace(string? name, string connectionString)
    {
        Id = Guid.NewGuid();
        ConnectionString = connectionString;
        Name = FillName(name, connectionString);
    }

    private string FillName(string? name, string connectionString) =>
        String.IsNullOrWhiteSpace(name)
            ? GetNamespaceNaming(connectionString)
            : name;

    private string GetNamespaceNaming(string connectionString)
    {
        const string startMarker = "Endpoint=sb://";
        char separator = ';';

        int startIndex = connectionString.IndexOf(startMarker, StringComparison.Ordinal);
        if (startIndex != -1)
        {
            startIndex += startMarker.Length;
            int endIndex = connectionString.IndexOf(separator, startIndex);
            if (endIndex != -1)
            {
                string endpoint = connectionString
                    .Substring(startIndex, endIndex - startIndex)
                    .Replace("/", "")
                    .Replace("\"", "");

                return endpoint;
            }
        }

        return string.Empty;
    }
}