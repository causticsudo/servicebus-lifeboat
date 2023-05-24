using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using ServiceBusLifeboat.Domain.Exceptions;
using static System.Console;

namespace ServiceBusLifeboat.Console.CommandHandlers;

/// <summary>
/// 
/// </summary>
public class ConnectOnNamespaceHandler
{
    private string connectedNamespace = String.Empty;
    private static ServiceBusAdministrationClient _adminClient;
    private static ServiceBusClient _client;



    /// <summary>
    /// 
    /// </summary>
    /// <param name="namespace"></param>
    public void Handle(string @namespace)
    {
        Clear();

        try
        {
            WriteLine($"TRYING_CONNECTION_FROM: {@namespace}");

            _adminClient = new(@namespace);
            _client = new(@namespace);

            WriteLine($"__NAMESPACE_CONNECTED: {@namespace}");
        }
        catch
        {
            throw new InvalidConnectionStringException();
        }

        WriteLine("Fazd qlqr coisa");
    }
}