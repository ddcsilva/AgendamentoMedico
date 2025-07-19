using Microsoft.EntityFrameworkCore;
using AgendamentoMedico.Domain.Entities;
using AgendamentoMedico.Domain.Interfaces;
using AgendamentoMedico.Infrastructure.Data;

namespace AgendamentoMedico.Infrastructure.Repositories;

/// <summary>
/// Implementação especializada do repositório de médicos
/// </summary>
/// <param name="context">Contexto do EF Core</param>
public class MedicoRepository(ApplicationDbContext context) : Repository<Medico>(context), IMedicoRepository
{

    /// <summary>
    /// Busca um médico pelo seu CRM
    /// </summary>
    public async Task<Medico?> BuscarPorCrmAsync(string crm, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(m => m.CRM == crm, cancellationToken);
    }

    /// <summary>
    /// Obtém um médico com suas consultas
    /// </summary>
    public async Task<Medico?> ObterComConsultasAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(m => m.Consultas)
                .ThenInclude(c => c.Paciente)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    /// <summary>
    /// Busca médicos por especialidade
    /// </summary>
    public async Task<IEnumerable<Medico>> BuscarPorEspecialidadeAsync(string especialidade, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(m => m.Especialidade.Contains(especialidade))
            .OrderBy(m => m.Nome)
            .ToListAsync(cancellationToken);
    }

        /// <summary>
    /// Verifica se um CRM já está em uso
    /// </summary>
    public async Task<bool> CrmJaExisteAsync(string crm, Guid? medicoId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(m => m.CRM == crm);

        if (medicoId.HasValue)
        {
            query = query.Where(m => m.Id != medicoId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    /// <summary>
    /// Obtém médicos com consultas no período especificado
    /// </summary>
    public async Task<IEnumerable<Medico>> ObterComConsultasNoPeriodoAsync(DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(m => m.Consultas.Where(c => c.DataHora >= dataInicio && c.DataHora <= dataFim))
                .ThenInclude(c => c.Paciente)
            .Where(m => m.Consultas.Any(c => c.DataHora >= dataInicio && c.DataHora <= dataFim))
            .OrderBy(m => m.Nome)
            .ToListAsync(cancellationToken);
    }
}