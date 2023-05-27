using System.CommandLine;
using System.CommandLine.Binding;
using sblboat.Model;

namespace sblboat.Binders;

public class ServiceBusNamespaceBinder : BinderBase<ServiceBusNamespace>
{
    private readonly Option<string> _name;
    private readonly Option<string> _connectionString;

    public ServiceBusNamespaceBinder(Option<string> name, Option<string> connectionString)
    {
        _name = name;
        _connectionString = connectionString;
    }

    protected override ServiceBusNamespace GetBoundValue(BindingContext bindingContext)
        => new ServiceBusNamespace(
            name: bindingContext.ParseResult.GetValueForOption(_name),
            connectionString: bindingContext.ParseResult.GetValueForOption(_connectionString)!);
}