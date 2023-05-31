using System.Security;
using ServiceBusLifeboat.Cli.Extensions;

namespace ServiceBusLifeboat.Cli.Model;

public class ServiceBusNamespace
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public SecureString ConnectionString { get; private set; }

    public ServiceBusNamespace(string name, SecureString connectionString)
    {
        Id = Guid.NewGuid();
        ConnectionString = connectionString;
        Name = FillName(name);
    }

    private string FillName(string name) =>
        name.IsNullOrWhiteSpace() ? String.Empty : name;
}