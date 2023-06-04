namespace ServiceBusLifeboat.Cli.Services.Token;

public interface ITokenService
{
    protected const uint DefaultExpirationInMinutes = 1;

    string GenerateJwtTokenAndWriteFile(uint expirationInMinutes = DefaultExpirationInMinutes);
    bool IsCurrentTokenOverdue(string? token);
    string RescueCurrentToken(string tokenPath = null);
}