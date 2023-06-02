namespace ServiceBusLifeboat.Cli.Services.Token;

public interface ITokenService
{
    private const int DefaultExpirationInMinutes = 1440;

    string GenerateToken(int expirationInMinutes = DefaultExpirationInMinutes);
}