namespace AgendamentoMedico.Infrastructure.Data;

/// <summary>
/// Interface para o contexto de banco de dados da aplicação
/// Expõe apenas operações necessárias, mantendo o DbContext encapsulado
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// Verifica se é possível conectar ao banco de dados
    /// Útil para health checks
    /// </summary>
    /// <returns>True se a conexão for bem-sucedida</returns>
    Task<bool> CanConnectAsync();

    /// <summary>
    /// Salva as mudanças no banco de dados de forma assíncrona
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Número de entidades afetadas</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executa as migrations pendentes
    /// </summary>
    /// <returns>Task representando a operação assíncrona</returns>
    Task MigrateAsync();

    /// <summary>
    /// Verifica se há migrations pendentes
    /// </summary>
    /// <returns>True se há migrations pendentes</returns>
    Task<bool> HasPendingMigrationsAsync();
}