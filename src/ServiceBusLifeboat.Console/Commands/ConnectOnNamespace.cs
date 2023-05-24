using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using ServiceBusLifeboat.Console.CommandHandlers;
using ServiceBusLifeboat.Console.Extensions;

namespace ServiceBusLifeboat.Console.Commands;

/// <summary>
/// 
/// </summary>
public class ConnectOnNamespace : Command
{
    private const string CommandName = "connect";


    /// <summary>
    /// 
    /// </summary>
    public ConnectOnNamespace(ConnectOnNamespaceHandler handler) : base(CommandName)
    {
        this.AddOptions(new Option[]
        {
            new Option<string>(
                aliases: new []{ "--namespace", "--connection-string"},
                description: "Connect to an azure service bus namespace, using a connection string")
        });

        Handler = CommandHandler.Create<string>(handler.Handle);

        AddValidator(symbol =>
        {
            if (symbol.Children.Count == 0)
            {
                this.InvokeAsync("--help").Wait();

                throw new InvalidOperationException("No arguments were provided. Use --help to get assistance.");
            }
        });
    }
}
