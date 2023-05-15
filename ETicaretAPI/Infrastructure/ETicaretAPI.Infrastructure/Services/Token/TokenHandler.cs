using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Application.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services
{
    public class TokenHandler : ITokenHandler
    {
        readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Application.DTOs.Token CreateAccessToken(int second)
        {
            Token token = new();
            //security key in simetriğini alıyoruz
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
            //şifrelenmiş kimliği oluşturuyoruz
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);
            //oluşturulan token ayarlarını veriyoruz
            token.Expiration = DateTime.UtcNow.AddMinutes(second);
            JwtSecurityToken securityToken = new(
                audience: _configuration["Token:Audience"],
                issuer: _configuration["Token:Issuer"],
                expires: token.Expiration,

                //üretilir üretilmez devreye giricek
                notBefore: DateTime.UtcNow,
                //şifresi burada saklanmış
                signingCredentials:signingCredentials          
                );

            //Token oluşturucu sınıfından bir örnek alalım.
            JwtSecurityTokenHandler tokenHandler = new();
           token.AccessToken= tokenHandler.WriteToken(securityToken);
            return token;

        }
    }
}
