using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using ServiceBusLifeboat.Console.Commands;
using ServiceBusLifeboat.Domain.Exceptions;
using static System.Console;

namespace ServiceBusLifeboat.Console.CommandHandlers;

/// <summary>
/// CommandHandler from <see cref="ConnectOnNamespace"/> command
/// </summary>
public class ConnectOnNamespaceHandler
{
    private string connectedNamespace = String.Empty;
    private static ServiceBusAdministrationClient _adminClient;
    private static ServiceBusClient? _client;

    /// <summary>
    /// Start a AzureServiceBus connection by client
    /// </summary>
    /// <param name="connectionString"></param>
    public void Handle(string connectionString)
    {
        Clear();

        var namespaceName = GetNamespaceNaming(connectionString);

        try
        {
            WriteLine($"TRYING_CONNECTION_FROM: {namespaceName}...");

            _client = (_client?.IsClosed ?? false)
                ? new ServiceBusClient(connectionString)
                : _client;

            _adminClient = new ServiceBusAdministrationClient(connectionString);
            _client = new ServiceBusClient(connectionString);

            connectedNamespace = connectionString;

            WriteLine($"__NAMESPACE_CONNECTED: {namespaceName}");

            ReadLine();
        }
        catch
        {
            throw new InvalidConnectionStringException();
        }
        finally
        {
            _client?.DisposeAsync();

            WriteLine("Client connection killed");
        }

        WriteLine("Doing anything");
    }

    private string GetNamespaceNaming(string connectionString)
    {
        const string startMarker = "Endpoint=sb://";
        char separator = ';';

        int startIndex = connectionString.IndexOf(startMarker);
        if (startIndex != -1)
        {
            startIndex += startMarker.Length;
            int endIndex = connectionString.IndexOf(separator, startIndex);
            if (endIndex != -1)
            {
                string endpoint = connectionString.Substring(startIndex, endIndex - startIndex);
                return endpoint;
            }
        }

        return string.Empty;
    }
}