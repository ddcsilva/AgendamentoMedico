using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AgendamentoMedico.Application.Interfaces;
using AgendamentoMedico.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AgendamentoMedico.Infrastructure.Services;

/// <summary>
/// Implementação do serviço de geração de tokens JWT
/// </summary>
public class JwtTokenService(IConfiguration configuration) : IJwtTokenService
{
    private readonly IConfiguration _configuration = configuration;

    /// <summary>
    /// Gera um token JWT para o usuário
    /// </summary>
    /// <param name="usuario">Usuário autenticado</param>
    /// <param name="claims">Claims do usuário</param>
    /// <returns>Token JWT como string</returns>
    public string GerarToken(Usuario usuario, IEnumerable<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey não configurada");
        var issuer = jwtSettings["Issuer"] ?? "AgendamentoMedico";
        var audience = jwtSettings["Audience"] ?? "AgendamentoMedico";
        var expireHours = int.Parse(jwtSettings["ExpireHours"] ?? "8");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Claims básicos do usuário
        var tokenClaims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, usuario.Email!),
            new(JwtRegisteredClaimNames.Name, usuario.Nome),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        // Adicionar claims customizados do usuário
        if (claims?.Any() == true)
        {
            tokenClaims.AddRange(claims);
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(tokenClaims),
            Expires = DateTime.UtcNow.AddHours(expireHours),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}