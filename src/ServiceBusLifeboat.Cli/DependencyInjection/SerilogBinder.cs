using System.CommandLine.Binding;
using Serilog;

namespace ServiceBusLifeboat.Cli.DependencyInjection;

public class SerilogBinder : BinderBase<ILogger>
{
    protected override ILogger GetBoundValue(BindingContext bindingContext) =>
        GetLoggerConfiguration().CreateLogger();

    LoggerConfiguration GetLoggerConfiguration()
    {
        LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console();

        return loggerConfiguration;
    }
}