using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ServiceBusLifeboat.Cli.Extensions;
using ServiceBusLifeboat.Cli.Extensions.Serilog;
using ServiceBusLifeboat.Cli.Services.File;
using ServiceBusLifeboat.Cli.Services.NetworkInterface;
using ServiceBusLifeboat.Cli.Services.Token.Enums;
using static ServiceBusLifeboat.Cli.GroupedConstants.Constants.ApplicationInformations;
using static ServiceBusLifeboat.Cli.GroupedConstants.Constants.TokenServiceMessages;

namespace ServiceBusLifeboat.Cli.Services.Token;

public class TokenService : ITokenService
{
    private readonly INetworkInterfaceService _networkInterfaceService;
    private readonly IFileService _fileService;
    private readonly ILogger _logger;
    private readonly JwtSecurityTokenHandler _jwtTokenHandler = new JwtSecurityTokenHandler();

    public TokenService(
        INetworkInterfaceService networkInterfaceService,
        IFileService fileService,
        ILogger logger)
    {
        _networkInterfaceService = networkInterfaceService;
        _fileService = fileService;
        _logger = logger;
    }

    public string RescueCurrentToken(out bool isActiveToken, string tokenPath = null)
    {
        var filePath = (tokenPath.IsNullOrWhiteSpace())
            ? _fileService.GetFilePath(DefaultConfigurationFolder, DefaultTokenFile)
            : tokenPath;

        var token = _fileService.GetJsonFile<Token>(filePath);
        var tokenString = token?.Content;

        if (tokenString.IsNullOrWhiteSpace())
        {
            _logger.Warning(TokenConfigurationNotFound);

            isActiveToken = false;

            return string.Empty;
        }

        var expectedSecretKey = GetTokenSecretKey();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(expectedSecretKey),
            ValidateIssuer = false,
            ValidateAudience = false
        };

        SecurityToken validatedToken;
        try
        {
            _jwtTokenHandler.ValidateToken(tokenString, validationParameters, out validatedToken);

            _logger.SuccessInformation(AuthenticatedToken);
        }
        catch (SecurityTokenValidationException ex)
        {
            _logger.Error(ex.Message, ex);

            isActiveToken = false;

            return string.Empty;
        }

        isActiveToken = validatedToken.ValidTo > DateTime.Now;

        return _jwtTokenHandler.WriteToken(validatedToken);
    }

    public string GenerateJwtTokenAndWriteFile(uint expirationInMinutes)
    {
        var secretKey = GetTokenSecretKey();
        var signingCredentials =
            new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);
        var expirationDate = DateTime.UtcNow.AddMinutes(expirationInMinutes);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[] { }),
            Expires = expirationDate,
            SigningCredentials = signingCredentials
        };

        _logger.Information(GeneratingToken);

        var token = _jwtTokenHandler.CreateToken(tokenDescriptor);
        var tokenString = _jwtTokenHandler.WriteToken(token);
        var filePath = _fileService.GetFilePath(DefaultConfigurationFolder, DefaultTokenFile);

        WriteTokenFile(filePath, tokenString, expirationInMinutes, expirationDate);

        _logger.Information(GeneratedToken);

        return tokenString;
    }

    private void WriteTokenFile(string tokenPath, string token, uint expirationInMinutes, DateTime expirationDate)
    {
        var file = new Token()
        {
            ExpirationDate = expirationDate,
            TokenDurationInMinutes = expirationInMinutes,
            Format = nameof(TokenFormat.JWT),
            GeneratedBy = Environment.UserName,
            Content = token,
            Path = tokenPath
        };

        _fileService.CreateJsonFile(file, tokenPath);
    }

    private byte[] GetTokenSecretKey()
    {
        var userInfo = EnvironmentExtensions.GetSystemUserInfo();
        var physicalAddressInfo = _networkInterfaceService.GetMacAddress();
        var combinedInfo = userInfo + physicalAddressInfo;
        using var sha256 = SHA256.Create();

        return sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedInfo));
    }
}