using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ServiceBusLifeboat.Cli.Extensions;
using ServiceBusLifeboat.Cli.Services.NetworkInterface;

namespace ServiceBusLifeboat.Cli.Services.Token;

public class TokenService : ITokenService
{
    private readonly INetworkInterfaceService _networkInterfaceService;

    public TokenService()
    {
        _networkInterfaceService = new NetworkInterfaceService();
    }

    public string GenerateToken(int expirationInMinutes)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var userInfo = EnvironmentExtensions.GetSystemUserInfo();
        var physicalAddressInfo = _networkInterfaceService.GetMacAddress();
        var combinedInfo = userInfo + physicalAddressInfo;
        using var sha256 = SHA256.Create();
        var secretKey = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedInfo));

        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[] {}),
            Expires = DateTime.UtcNow.AddMinutes(expirationInMinutes),
            SigningCredentials = signingCredentials
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }
}