namespace sblboat.Model;

public class ServiceBusNamespaceStorage
{
    public IEnumerable<ServiceBusNamespace>? SavedNamespaces { get; private set; }
    public ServiceBusNamespace CurrentNamespace { get; private set; }

    public ServiceBusNamespaceStorage(
        ServiceBusNamespace currentNamespace,
        IEnumerable<ServiceBusNamespace> savedNamespaces)
    {
        CurrentNamespace = currentNamespace;
        SavedNamespaces = savedNamespaces;
    }
}