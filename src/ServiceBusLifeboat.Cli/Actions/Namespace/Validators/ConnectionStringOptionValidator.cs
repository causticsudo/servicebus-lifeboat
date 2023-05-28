using System.CommandLine.Parsing;
using ServiceBusLifeboat.Cli.Extensions;
using static ServiceBusLifeboat.Cli.GroupedConstants.RegexPattern;

namespace ServiceBusLifeboat.Cli.Actions.Namespace.Validators;

//Todo: Maybe this must be a interface
public class ConnectionStringOptionValidator
{
    private readonly ArgumentResult _result;

    public ConnectionStringOptionValidator(ArgumentResult result)
    {
        _result = result;
    }

    public string ParseResult()
    {
        if (_result.Tokens.Count == 0)
        {
            _result.ErrorMessage = $"[--connection-string] option is required";
            return null!;
        }

        var connectionString = _result.Tokens.Single().Value;
        if (connectionString.IsNullOrWhiteSpace())
        {
            _result.ErrorMessage = $"[--connection-string] value should not be null or empty";
            return null!;
        }

        if (connectionString.IsMatchFromPattern(ServiceBuConnectionStringPattern))
        {
            return connectionString;
        }
        else
        {
            _result.ErrorMessage = $"[--connection-string] is in the wrong format";
            return null!;
        }
    }
}