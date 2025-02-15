using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PowerPath.Domain.Interfaces.Security;

namespace PowerPath.Infra.Security
{
    public class JWTSecurity : IJWTSecurity
    {
        private readonly string _chaveSecreta;

        public JWTSecurity()
        {
            _chaveSecreta = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")!;
        }

        public string GerarToken(string nome)
        {
            Claim[] claims =
            [
                new Claim(ClaimTypes.Name, nome),
                new Claim(ClaimTypes.Role, "Default")
            ];

            SymmetricSecurityKey chave = new(Encoding.UTF8.GetBytes(_chaveSecreta));
            SigningCredentials credenciais = new(chave, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new(
                issuer: "PowerPath",
                audience: "Default",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credenciais
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
