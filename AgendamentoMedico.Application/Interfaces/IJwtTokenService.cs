using AgendamentoMedico.Domain.Entities;
using System.Security.Claims;

namespace AgendamentoMedico.Application.Interfaces;

/// <summary>
/// Interface para serviço de geração de tokens JWT
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Gera um token JWT para o usuário
    /// </summary>
    /// <param name="usuario">Usuário autenticado</param>
    /// <param name="claims">Claims do usuário</param>
    /// <returns>Token JWT como string</returns>
    string GerarToken(Usuario usuario, IEnumerable<Claim> claims);
}