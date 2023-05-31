using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ServiceBusLifeboat.Cli.Services;

public static class TokenService
{
    private const int DefaultExpirationInMinutes = 1440;

    public static string GenerateToken(int expirationInMinutes = DefaultExpirationInMinutes)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var userInfo = EnvironmentService.GetSystemUserInfo();
        var physicalAddressInfo = NetworkInterfaceService.GetMacAddress();
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