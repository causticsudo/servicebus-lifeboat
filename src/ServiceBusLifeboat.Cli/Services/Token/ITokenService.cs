namespace ServiceBusLifeboat.Cli.Services.Token;

public interface ITokenService
{
    const uint DefaultExpirationInMinutes = 1;

    string GenerateJwtTokenAndWriteFile(uint expirationInMinutes = DefaultExpirationInMinutes);
    string RescueCurrentToken(out bool isActiveToken, string tokenPath = null);
}