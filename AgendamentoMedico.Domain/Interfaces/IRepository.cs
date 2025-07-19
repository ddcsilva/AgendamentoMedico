using System.Linq.Expressions;
using AgendamentoMedico.Domain.Entities;

namespace AgendamentoMedico.Domain.Interfaces;

/// <summary>
/// Interface base para repositórios genéricos
/// </summary>
/// <typeparam name="TEntity">Tipo da entidade que herda de Entity</typeparam>
public interface IRepository<TEntity> where TEntity : Entity
{
    /// <summary>
    /// Obtém uma entidade por seu identificador
    /// </summary>
    /// <param name="id">Identificador da entidade</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>A entidade encontrada ou null se não existir</returns>
    Task<TEntity?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém todas as entidades
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Coleção de todas as entidades</returns>
    Task<IEnumerable<TEntity>> ObterTodosAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca entidades que atendem ao predicado especificado
    /// </summary>
    /// <param name="predicate">Expressão de filtro</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Coleção de entidades que atendem ao filtro</returns>
    Task<IEnumerable<TEntity>> BuscarAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se existe alguma entidade que atende ao predicado
    /// </summary>
    /// <param name="predicate">Expressão de filtro</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se existir pelo menos uma entidade que atende ao predicado</returns>
    Task<bool> ExisteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adiciona uma nova entidade
    /// </summary>
    /// <param name="entity">Entidade a ser adicionada</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>A entidade adicionada</returns>
    Task<TEntity> AdicionarAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza uma entidade existente
    /// </summary>
    /// <param name="entity">Entidade a ser atualizada</param>
    void Atualizar(TEntity entity);

    /// <summary>
    /// Remove uma entidade
    /// </summary>
    /// <param name="entity">Entidade a ser removida</param>
    void Remover(TEntity entity);

    /// <summary>
    /// Remove uma entidade por seu identificador
    /// </summary>
    /// <param name="id">Identificador da entidade a ser removida</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se a entidade foi removida, False se não foi encontrada</returns>
    Task<bool> RemoverPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Salva todas as alterações pendentes no contexto
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Número de entidades afetadas</returns>
    Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken = default);
}