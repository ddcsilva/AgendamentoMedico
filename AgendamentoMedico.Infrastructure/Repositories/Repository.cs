using System.Linq.Expressions;
using AgendamentoMedico.Domain.Entities;
using AgendamentoMedico.Domain.Interfaces;
using AgendamentoMedico.Infrastructure.Data;

namespace AgendamentoMedico.Infrastructure.Repositories;

/// <summary>
/// Implementação base genérica dos repositórios
/// </summary>
/// <typeparam name="TEntity">Tipo da entidade que herda de Entity</typeparam>
/// <param name="context">Contexto do EF Core</param>
public class Repository<TEntity>(ApplicationDbContext context) : IRepository<TEntity> where TEntity : Entity
{
    protected readonly ApplicationDbContext _context = context;
    protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    /// <summary>
    /// Obtém uma entidade por seu identificador
    /// </summary>
    public virtual async Task<TEntity?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync([id], cancellationToken);
    }

    /// <summary>
    /// Obtém todas as entidades
    /// </summary>
    public virtual async Task<IEnumerable<TEntity>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Busca entidades que atendem ao predicado especificado
    /// </summary>
    public virtual async Task<IEnumerable<TEntity>> BuscarAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Verifica se existe alguma entidade que atende ao predicado
    /// </summary>
    public virtual async Task<bool> ExisteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(predicate, cancellationToken);
    }

    /// <summary>
    /// Adiciona uma nova entidade
    /// </summary>
    public virtual async Task<TEntity> AdicionarAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = await _dbSet.AddAsync(entity, cancellationToken);
        return entry.Entity;
    }

    /// <summary>
    /// Atualiza uma entidade existente
    /// </summary>
    public virtual void Atualizar(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    /// <summary>
    /// Remove uma entidade
    /// </summary>
    public virtual void Remover(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    /// <summary>
    /// Remove uma entidade por seu identificador
    /// </summary>
    public virtual async Task<bool> RemoverPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await ObterPorIdAsync(id, cancellationToken);
        if (entity == null)
            return false;

        Remover(entity);
        return true;
    }

    /// <summary>
    /// Salva todas as alterações pendentes no contexto
    /// </summary>
    public virtual async Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}