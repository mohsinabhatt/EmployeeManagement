using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SharedLibrary
{
    public sealed class AuthHandler
    {
        public static string GenerateToken(string secret, ClaimsIdentity claims, DateTime expiry)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = expiry,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(token);
            return encryptedToken;
        }


        public static JwtSecurityToken ReadToken(string jwt)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(jwt);
        }


        public static bool ValidateTokenForRefresh(string token, string key)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidateLifetime = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false
            };
            var claimsPrincipal = jwtTokenHandler.ValidateToken(token, parameters, out _);

            if (claimsPrincipal == null)
                throw new SecurityTokenException("Invalid token");

            return true;
        }


        public static string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
