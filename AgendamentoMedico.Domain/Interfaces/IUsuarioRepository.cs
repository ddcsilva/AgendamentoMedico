using AgendamentoMedico.Domain.Entities;

namespace AgendamentoMedico.Domain.Interfaces;

/// <summary>
/// Interface especializada para repositório de usuários
/// Segue o padrão das demais interfaces do sistema
/// </summary>
public interface IUsuarioRepository
{
    /// <summary>
    /// Busca um usuário pelo seu email
    /// </summary>
    /// <param name="email">Email do usuário</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>O usuário encontrado ou null se não existir</returns>
    Task<Usuario?> BuscarPorEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca um usuário pelo seu ID
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>O usuário encontrado ou null se não existir</returns>
    Task<Usuario?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém um usuário com todos os seus perfis/claims
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>O usuário com seus perfis ou null se não existir</returns>
    Task<Usuario?> ObterComPerfisAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém um usuário com perfis pelo email
    /// </summary>
    /// <param name="email">Email do usuário</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>O usuário com seus perfis ou null se não existir</returns>
    Task<Usuario?> ObterComPerfilsPorEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca usuários por nome (busca parcial)
    /// </summary>
    /// <param name="nome">Nome ou parte do nome do usuário</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de usuários cujo nome contém o termo especificado</returns>
    Task<IEnumerable<Usuario>> BuscarPorNomeAsync(string nome, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém todos os usuários ativos
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de usuários ativos</returns>
    Task<IEnumerable<Usuario>> ObterUsuariosAtivosAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca usuários que possuem um claim específico
    /// </summary>
    /// <param name="tipoClaim">Tipo do claim</param>
    /// <param name="valorClaim">Valor do claim (opcional)</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de usuários que possuem o claim</returns>
    Task<IEnumerable<Usuario>> BuscarPorClaimAsync(string tipoClaim, string? valorClaim = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se um email já está em uso
    /// </summary>
    /// <param name="email">Email a ser verificado</param>
    /// <param name="usuarioId">ID do usuário a ser excluído da verificação (para atualização)</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se o email já estiver em uso</returns>
    Task<bool> EmailJaExisteAsync(string email, Guid? usuarioId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém usuários com último login em um período específico
    /// </summary>
    /// <param name="dataInicio">Data de início do período</param>
    /// <param name="dataFim">Data de fim do período</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de usuários que fizeram login no período</returns>
    Task<IEnumerable<Usuario>> ObterUsuariosComLoginNoPeriodoAsync(DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém usuários inativos há mais de X dias
    /// </summary>
    /// <param name="diasInatividade">Número de dias de inatividade</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de usuários inativos há mais de X dias</returns>
    Task<IEnumerable<Usuario>> ObterUsuariosInativosAsync(int diasInatividade, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adiciona um usuário
    /// </summary>
    /// <param name="usuario">Usuário a ser adicionado</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>O usuário adicionado</returns>
    Task<Usuario> AdicionarAsync(Usuario usuario, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza um usuário
    /// </summary>
    /// <param name="usuario">Usuário a ser atualizado</param>
    void Atualizar(Usuario usuario);

    /// <summary>
    /// Remove um usuário (soft delete - marca como inativo)
    /// </summary>
    /// <param name="usuario">Usuário a ser removido</param>
    void Remover(Usuario usuario);

    /// <summary>
    /// Remove um usuário por ID (soft delete)
    /// </summary>
    /// <param name="id">ID do usuário a ser removido</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se o usuário foi removido, False se não foi encontrado</returns>
    Task<bool> RemoverPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Salva todas as alterações pendentes
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Número de entidades afetadas</returns>
    Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken = default);
}