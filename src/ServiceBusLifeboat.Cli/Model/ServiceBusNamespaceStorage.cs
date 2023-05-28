using System.Text.Json.Serialization;

namespace ServiceBusLifeboat.Cli.Model;

public class ServiceBusNamespaceStorage
{
    public IEnumerable<ServiceBusNamespace>? StoragedServiceBusNamespaces { get; private set; }
    public ServiceBusNamespace CurrentNamespace { get; private set; }

    [JsonConstructor]
    public ServiceBusNamespaceStorage(
        ServiceBusNamespace currentNamespace,
        IEnumerable<ServiceBusNamespace> storagedServiceBusNamespaces)
    {
        CurrentNamespace = currentNamespace;
        StoragedServiceBusNamespaces = storagedServiceBusNamespaces;
    }

    public void TryInsertNamespaceOnStorage(ServiceBusNamespace serviceBusNamespace)
    {
        if (StoragedServiceBusNamespaces.Any(x => x.Id == serviceBusNamespace.Id))
        {
            throw new Exception("nao pode");
        }

        StoragedServiceBusNamespaces.ToList().Add(serviceBusNamespace);
    }

    public void SetCurrentNamespace(ServiceBusNamespace serviceBusNamespace)
    {
        CurrentNamespace = serviceBusNamespace;
    }
}