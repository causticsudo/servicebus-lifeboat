namespace ServiceBusLifeboat.Cli.GroupedConstants;

public partial class Constants
{
    public class TokenServiceMessages
    {
        public const string GeneratingToken = "generating new access token...";
        public const string GeneratedToken = "access token generated with success";
        public const string AuthenticatedToken = "token authenticated !";
        public const string TokenConfigurationNotFound = "token configuration file not found";
    }
}