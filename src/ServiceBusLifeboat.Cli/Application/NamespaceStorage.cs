using ServiceBusLifeboat.Cli.Model;

namespace ServiceBusLifeboat.Cli.Application;

public class NamespaceStorage
{
    public IEnumerable<ServiceBusNamespace>? Namespaces { get; private set; }

    public NamespaceStorage(IEnumerable<ServiceBusNamespace> namespaces)
    {
        Namespaces = namespaces;
    }

    public void TryInsertNamespaceOnStorage(ServiceBusNamespace serviceBusNamespace)
    {
        if (Namespaces.Any(x => x.Id == serviceBusNamespace.Id))
        {
            throw new Exception("nao pode");
        }

        Namespaces.ToList().Add(serviceBusNamespace);
    }
}