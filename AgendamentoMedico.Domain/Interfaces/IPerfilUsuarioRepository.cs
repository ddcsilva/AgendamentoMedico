using AgendamentoMedico.Domain.Entities;

namespace AgendamentoMedico.Domain.Interfaces;

/// <summary>
/// Interface especializada para repositório de perfis de usuário
/// </summary>
public interface IPerfilUsuarioRepository : IRepository<PerfilUsuario>
{
    /// <summary>
    /// Busca perfis de um usuário específico
    /// </summary>
    /// <param name="usuarioId">ID do usuário</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de perfis do usuário</returns>
    Task<IEnumerable<PerfilUsuario>> BuscarPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca perfis por tipo de claim
    /// </summary>
    /// <param name="tipoClaim">Tipo do claim</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de perfis do tipo especificado</returns>
    Task<IEnumerable<PerfilUsuario>> BuscarPorTipoClaimAsync(string tipoClaim, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca perfis por tipo e valor de claim
    /// </summary>
    /// <param name="tipoClaim">Tipo do claim</param>
    /// <param name="valorClaim">Valor do claim</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de perfis que correspondem ao tipo e valor</returns>
    Task<IEnumerable<PerfilUsuario>> BuscarPorClaimAsync(string tipoClaim, string valorClaim, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém um perfil específico de um usuário
    /// </summary>
    /// <param name="usuarioId">ID do usuário</param>
    /// <param name="tipoClaim">Tipo do claim</param>
    /// <param name="valorClaim">Valor do claim</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>O perfil encontrado ou null se não existir</returns>
    Task<PerfilUsuario?> ObterPerfilUsuarioAsync(Guid usuarioId, string tipoClaim, string valorClaim, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se um usuário possui um claim específico
    /// </summary>
    /// <param name="usuarioId">ID do usuário</param>
    /// <param name="tipoClaim">Tipo do claim</param>
    /// <param name="valorClaim">Valor do claim (opcional)</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se o usuário possui o claim</returns>
    Task<bool> UsuarioPossuiClaimAsync(Guid usuarioId, string tipoClaim, string? valorClaim = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove todos os perfis de um usuário
    /// </summary>
    /// <param name="usuarioId">ID do usuário</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Número de perfis removidos</returns>
    Task<int> RemoverTodosPerfilsUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove perfis específicos de um usuário por tipo
    /// </summary>
    /// <param name="usuarioId">ID do usuário</param>
    /// <param name="tipoClaim">Tipo do claim a ser removido</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Número de perfis removidos</returns>
    Task<int> RemoverPerfilsPorTipoAsync(Guid usuarioId, string tipoClaim, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém estatísticas de uso de claims
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dicionário com tipo de claim e quantidade de usos</returns>
    Task<Dictionary<string, int>> ObterEstatisticasClaimsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca usuários que possuem múltiplos claims específicos
    /// </summary>
    /// <param name="claims">Lista de claims (tipo, valor) que o usuário deve possuir</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de perfis de usuários que possuem todos os claims especificados</returns>
    Task<IEnumerable<PerfilUsuario>> BuscarUsuariosComMultiplosClaimsAsync(IEnumerable<(string Tipo, string Valor)> claims, CancellationToken cancellationToken = default);
}